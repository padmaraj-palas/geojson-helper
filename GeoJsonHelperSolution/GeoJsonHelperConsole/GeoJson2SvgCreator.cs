using CSVHelper;
using GeoJsonHelper;
using GeoJsonHelper.GeoJsonObjects;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoPositioning;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoJson2SvgCreator
    {
        public static async Task Create(string geoJsonPath, string poisPath)
        {
            IIMDFGeoJsonService geoJsonService = new IMDFGeoJsonService();
            var geoJson = await geoJsonService.LoadAsync(geoJsonPath);
            var records = await CSVParser.ParseAsync(poisPath);
            OnPoiRecordLoadCompleted(records, geoJsonService);
        }

        public static void Create(Vector2[] rootVertices, IList<(Vector2[] vertices, SitumPoiData poi, string groupName)> mapping)
        {
            SvgDocument svgDocument = new();
            CreateRoot(svgDocument, rootVertices);

            foreach (var item in mapping)
            {
                DrawFeature(svgDocument, item);
            }

            svgDocument.Write(Path.GetFullPath("./results.svg"));
        }

        private static void CreateRoot(SvgDocument svgDocument, Vector2[] vertices)
        {
            var (min, max, size) = vertices.GetBounds();
            svgDocument.ViewBox = new SvgViewBox((float)min.X, -(float)max.Y, (float)size.X, (float)size.Y);
            SvgRectangle svgRectangle = new SvgRectangle();
            svgRectangle.X = (float)min.X;
            svgRectangle.Y = -(float)max.Y;
            svgRectangle.Width = (float)size.X;
            svgRectangle.Height = (float)size.Y;
            svgRectangle.Stroke = SvgPaintServer.None;
            svgRectangle.Fill = new SvgColourServer(Color.Black);
            svgDocument.Children.Add(svgRectangle);

            if (vertices == null || vertices.Length == 0)
            {
                return;
            }

            var color = ColorUtility.GetRandomColor();
            color = Color.FromArgb((byte)(color.R * 0.05f), (byte)(color.G * 0.05f), (byte)(color.B * 0.05f));
            var contentRoot = new SvgGroup();
            contentRoot.SetAndForceUniqueID("Static Root");
            svgDocument.Children.Add(contentRoot);
            var svgPolygon = new SvgPolygon();
            svgPolygon.Stroke = SvgPaintServer.None;
            svgPolygon.Fill = new SvgColourServer(color);
            SvgPointCollection svgUnits = new SvgPointCollection();
            svgUnits.AddRange(vertices.SelectMany(p => new SvgUnit[] { new SvgUnit((float)p.X), new SvgUnit(-(float)p.Y) }));
            svgPolygon.Points = svgUnits;
            contentRoot.Children.Add(svgPolygon);
        }

        public static void DrawFeature(SvgDocument svgRoot, (Vector2[] vertices, SitumPoiData poi, string groupName) mapping)
        {
            if (mapping.vertices == null || mapping.vertices.Length == 0)
            {
                return;
            }

            var contentRoot = svgRoot.GetElementById(mapping.groupName);
            if (contentRoot == null)
            {
                contentRoot = new SvgGroup();
                contentRoot.SetAndForceUniqueID(mapping.groupName);
                svgRoot.Children.Add(contentRoot);
            }

            var poiRoot = new SvgGroup();
            poiRoot.SetAndForceUniqueID($"{mapping.poi.Id}");
            contentRoot.Children.Add(poiRoot);

            Color color = ColorUtility.GetRandomColor();
            var svgPolygon = new SvgPolygon();
            svgPolygon.Stroke = SvgPaintServer.None;
            svgPolygon.Fill = new SvgColourServer(color);
            SvgPointCollection svgUnits = new SvgPointCollection();
            svgUnits.AddRange(mapping.vertices.SelectMany(p => new SvgUnit[] { new SvgUnit((float)p.X), new SvgUnit(-(float)p.Y) }));
            svgPolygon.Points = svgUnits;
            poiRoot.Children.Add(svgPolygon);

            SvgUnitCollection x = new SvgUnitCollection
            {
                svgPolygon.Bounds.X + svgPolygon.Bounds.Width / 2f
            };

            SvgUnitCollection y = new SvgUnitCollection
            {
                svgPolygon.Bounds.Y + svgPolygon.Bounds.Height / 2f
            };

            var svgText = new SvgText($"{mapping.poi.Name}");
            svgText.Stroke = SvgPaintServer.None;
            svgText.Fill = new SvgColourServer(Color.FromArgb((byte)(color.R * 0.75f), (byte)(color.G * 0.75f), (byte)(color.B * 0.75f)));
            svgText.TextAnchor = SvgTextAnchor.Middle;
            svgText.X = x;
            svgText.Y = y;
            svgText.FontSize = new SvgUnit((float)Math.Min(svgPolygon.Bounds.Width, svgPolygon.Bounds.Height) / 10f);
            poiRoot.Children.Add(svgText);
        }



        public static void OnPoiRecordLoadCompleted(CsvRecord[] records, IIMDFGeoJsonService geoJsonService)
        {
            if (records == null || records.Length == 0)
            {
                return;
            }

            var json = records.ToJson();
            var pois = PoiJsonSerializer.Deserialize<SitumPoiData[]>(json).Where(p => p.Position.FloorId == 109);

            var building = geoJsonService.Buildings.Values.FirstOrDefault();
            var level = geoJsonService.Levels.Values.FirstOrDefault(l => l.Properties.Ordinal == 3);

            var displayPoint = building.Properties.Display_point.Coordinates;
            var origin = new GeoPosition((double)displayPoint.Latitude, (double)displayPoint.Longitude);

            var units = geoJsonService.Units.Values
                .Where(u => u.Properties.Level_id == level.Id);

            var cats = new string[] { "walkway", "road", "parking" };
            units = units.Where(u => u.Properties.Category == "road")
                .Concat(units.Where(u => u.Properties.Category == "walkway"))
                .Concat(units.Where(u => u.Properties.Category == "parking"))
                .Concat(units.Where(u => !cats.Contains(u.Properties.Category)));

            var mapping = MapGeoJsonAndPoiMock(units, pois, origin);
            GeoJson2SvgCreator.Create(geoJsonService.Venues.Values.FirstOrDefault().GetVerticesFromFeature(origin)[0], mapping);

            var bounds = geoJsonService.Venues.Values.FirstOrDefault().GetVerticesFromFeature(origin)[0].GetBounds();
            var lowerLeftCorner = bounds.min;
            var upperLeftCorner = lowerLeftCorner + Vector2.Up * bounds.size.Y;
            var upperRightCorner = bounds.max;
            var lowerRightCorner = lowerLeftCorner + Vector2.Right * bounds.size.X;

            var angle = Vector2.Angle(Vector2.Up, Vector2.Right);

            var lf = GeoPosition.GetGeoPositionFromMetricPosition(origin, lowerLeftCorner);
            var ul = GeoPosition.GetGeoPositionFromMetricPosition(origin, upperLeftCorner);
            var ur = GeoPosition.GetGeoPositionFromMetricPosition(origin, upperRightCorner);
            var lr = GeoPosition.GetGeoPositionFromMetricPosition(origin, lowerRightCorner);
        }

        public static IList<(Vector2[] vertices, SitumPoiData poi, string groupName)> MapGeoJsonAndPoiMock(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<SitumPoiData> pois, GeoPosition origin)
        {
            IList<(Vector2[], SitumPoiData, string)> mapping = new List<(Vector2[], SitumPoiData, string)>();

            int index = 0;
            foreach (var feature in features)
            {
                index++;
                var poiData = new SitumPoiData
                {
                    BuildingId = 29,
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
                    }
                };

                var vertices = feature.GetVerticesFromFeature(origin)[0];
                mapping.Add((vertices, poiData, string.IsNullOrEmpty(feature.Properties.Category) ? "unknown" : feature.Properties.Category));
            }

            return mapping;
        }

        public static IList<(Vector2[] vertices, SitumPoiData poi, string groupName)> MapGeoJsonAndPoiTries(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<SitumPoiData> pois, GeoPosition origin)
        {
            int poiCount = pois.Count();
            IList<(Vector2[], SitumPoiData, string)> mapping = new List<(Vector2[], SitumPoiData, string)>();

            IDictionary<GeoJsonFeature, IList<SitumPoiData>> featureToPoiMapping = new Dictionary<GeoJsonFeature, IList<SitumPoiData>>();
            IList<SitumPoiData> poiNotBelonging = new List<SitumPoiData>();

            foreach (var poiData in pois)
            {
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
                        mapping.Add((vertices, poiData, "PoiRoot"));
                        break;
                    }
                }

                if (!isInside)
                {
                    poiNotBelonging.Add(poiData);
                }
            }

            var multipleMapping = string.Join("\n", featureToPoiMapping
                .Where(kvp => kvp.Value.Count > 1)
                .SelectMany(kvp => kvp.Value).Select((poi, index) => $"{index + 1}.\t{poi.Id}, {poi.Name}"));

            var noMapping = string.Join("\n", poiNotBelonging.Select((poi, index) => $"{index + 1}.\t{poi.Id}, {poi.Name}"));
            File.WriteAllText(Path.GetFullPath("./multipleMapping.txt"), multipleMapping);
            File.WriteAllText(Path.GetFullPath("./NoMapping.txt"), noMapping);
            return mapping;
        }

        public static IList<(GeoJsonFeature feature, SitumPoiData poi, string groupName)> MapGeoJsonAndPoi(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<SitumPoiData> pois, GeoPosition origin)
        {
            IList<(GeoJsonFeature, SitumPoiData, string)> mapping = new List<(GeoJsonFeature, SitumPoiData, string)>();

            foreach (var feature in features)
            {
                foreach (SitumPoiData poiData in pois)
                {
                    var poiGeoPosition = new GeoPosition(poiData.Position.Latitude, poiData.Position.Longitude);
                    var poiPosition = GeoPosition.GetPositionInMeters(poiGeoPosition, origin);
                    var vertices = feature.GetVerticesFromFeature(origin);
                    var verticeList = vertices.ToList();
                    verticeList.RemoveAt(verticeList.Count - 1);
                    var polyBound = new PolyBound();
                    polyBound.SetPoints(verticeList[0].ToList());
                    if (polyBound.IsPointInside(poiPosition))
                    {
                        mapping.Add((feature, poiData, string.IsNullOrEmpty(feature.Properties.Category) ? "unknown" : feature.Properties.Category));
                        break;
                    }
                }
            }

            return mapping;
        }
    }
}
