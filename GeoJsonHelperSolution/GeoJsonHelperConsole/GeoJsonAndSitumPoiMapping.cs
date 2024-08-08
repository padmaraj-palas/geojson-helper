using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoPositioning;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoJsonAndSitumPoiMapping
    {
        private const string chars = "|/-\\";

        public static IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData> GenerateMockPoisForGeoJson(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, GeoPosition origin)
        {
            IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData> mapping = new Dictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData>();

            int index = 0;
            foreach (var feature in features)
            {
                index++;
                var poiData = new SitumPoiData
                {
                    BuildingId = 29,
                    CategoryName = string.IsNullOrEmpty(feature.Properties.Category) ? "unknown" : feature.Properties.Category,
                    Id = index,
                    Name = $"{index}",//$"Poi {index}",
                    Type = "POI",
                    Position = new SitumPoiData.Pos
                    {
                        FloorId = 109,
                        Latitude = (double)feature.Properties.Display_point.Coordinates.Latitude,
                        Longitude = (double)feature.Properties.Display_point.Coordinates.Longitude,
                        Georeferences = new SitumPoiData.GeoRef
                        {
                            Latitude = (double)feature.Properties.Display_point.Coordinates.Latitude,
                            Longitude = (double)feature.Properties.Display_point.Coordinates.Longitude
                        }
                    },
                    FillColor = ColorTranslator.FromHtml(feature.Properties.FillColor)
                };

                var vertices = feature.GetVerticesFromFeature(origin)[0];
                mapping.Add(feature, poiData);
            }

            return mapping;
        }

        public static (IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData> matches, IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, IList<SitumPoiData>> multiMatch, IList<SitumPoiData> notMatched) MapGeoJsonAndPoi(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<SitumPoiData> pois, GeoPosition origin)
        {
            IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, IList<SitumPoiData>> featureToPoiMapping = new Dictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, IList<SitumPoiData>>();
            IList<SitumPoiData> poiNotBelonging = new List<SitumPoiData>();

            int totalPois = pois.Count();
            int iteration = 0;
            foreach (var poiData in pois)
            {
                iteration++;
                Console.Write($"\rCompleted: {(int)(((float)iteration / totalPois) * 100f)} %\t{chars[iteration % chars.Length]}\t");
                var poiGeoPosition = new GeoPosition(poiData.Position.Georeferences.Latitude, poiData.Position.Georeferences.Longitude);
                var poiPosition = GeoPosition.GetPositionInMeters(poiGeoPosition, origin);

                bool isInside = false;
                foreach (var feature in features)
                {
                    var vertices = feature.GetVerticesFromFeature(origin)[0];
                    var clonedVertices = new Vector2[vertices.Length - 1];
                    Array.Copy(vertices, clonedVertices, clonedVertices.Length);
                    var triangles = EarClippingTriangulator.GetTriangles(clonedVertices);

                    var triList = new List<Vector2[]>();
                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        var triangulatedVertices = new Vector2[3];
                        for (int j = 0; j < 3; j++)
                        {
                            try
                            {
                                triangulatedVertices[j] = clonedVertices[triangles[i + j]];
                            }
                            catch (Exception ex)
                            {
                                int index = i + j;
                                Console.WriteLine(index);
                            }
                        }

                        triList.Add(triangulatedVertices);
                    }

                    foreach (var tries in triList)
                    {
                        var polyBound = new PolyBound();
                        polyBound.SetPoints(tries.ToList());
                        if (polyBound.IsPointInside(poiPosition))
                        {
                            isInside = true;
                            break;
                        }
                    }

                    if (isInside)
                    {
                        IList<SitumPoiData> poiMapping;
                        if (!featureToPoiMapping.TryGetValue(feature, out poiMapping))
                        {
                            poiMapping = new List<SitumPoiData>();
                            featureToPoiMapping.Add(feature, poiMapping);
                        }

                        poiMapping.Add(poiData);
                        break;
                    }
                }

                if (!isInside)
                {
                    poiNotBelonging.Add(poiData);
                }
            }

            var oneToOneMatches = featureToPoiMapping
                .Where(kvp => kvp.Value != null && kvp.Value.Count == 1)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value[0]);

            var multipleMapping = featureToPoiMapping
                .Where(kvp => kvp.Value != null && kvp.Value.Count > 1)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return (oneToOneMatches, multipleMapping, poiNotBelonging);
        }

        public static Task<(IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData> matches, IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, IList<SitumPoiData>> multiMatch, IList<SitumPoiData> notMatched)> MapGeoJsonAndPoiAsync(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<SitumPoiData> pois, GeoPosition origin)
        {
            var taskCompletionSource = new TaskCompletionSource<(IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, SitumPoiData> matches, IDictionary<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>, IList<SitumPoiData>> multiMatch, IList<SitumPoiData> notMatched)>();

            Task.Run(() =>
            {
                var result = MapGeoJsonAndPoi(features, pois, origin);
                taskCompletionSource.SetResult(result);
            });

            return taskCompletionSource.Task;
        }
    }
}
