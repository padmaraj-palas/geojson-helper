using GeoJsonHelper;
using GeoPositioning;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class FilterPoiBasedOnGeoLocationMask
    {
        private static readonly JsonSerializer _serializer;

        static FilterPoiBasedOnGeoLocationMask()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };

            _serializer.Converters.Add(new StringEnumConverter());
        }

        public static async Task<IList<PoiData>> Filter(string svgPath, string geoJsonPath, (List<PoiData> pois, List<PoiMetaData> poiMetaData) poiData)
        {
            if (!File.Exists(svgPath) || !File.Exists(geoJsonPath))
            {
                return new List<PoiData>();
            }

            IIMDFGeoJsonService geoJsonService = new IMDFGeoJsonService();
            var geoJson = await geoJsonService.LoadAsync(geoJsonPath);

            var building = geoJsonService.Buildings.Values.FirstOrDefault();
            var displayPoint = building.Properties.Display_point.Coordinates;
            var origin = new GeoPosition((double)displayPoint.Latitude, (double)displayPoint.Longitude);

            SvgDocument svgDocument = SvgDocument.Open(svgPath);
            var groups = svgDocument.Children.Where(c => c is SvgGroup).Select(c => c as SvgGroup).ToList();
            var maskGroup = groups.FirstOrDefault(g => g.ID == "Mask");

            if (maskGroup == null)
            {
                return new List<PoiData>();
            }

            var mask = maskGroup.Children.FirstOrDefault(c => c is SvgPath) as SvgPath;

            IList<Vector2> vertices = new List<Vector2>();
            Vector2 start = new Vector2(0, 0);
            for(int i = 0; i < mask.PathData.Count; i++)
            {
                start = new Vector2(mask.PathData[i].End.X, mask.PathData[i].End.Y) + start;
                vertices.Add(new Vector2(start.X, -start.Y));
            }

            var triangles = EarClippingTriangulator.GetTriangles(vertices.ToArray());

            var triList = new List<Vector2[]>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                var triangulatedVertices = new Vector2[3];
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        triangulatedVertices[j] = vertices[triangles[i + j]];
                    }
                    catch (Exception ex)
                    {
                        int index = i + j;
                        Console.WriteLine(index);
                    }
                }

                triList.Add(triangulatedVertices);
            }

            var insidePois = new List<PoiMetaData>();
            foreach(var poi in poiData.poiMetaData)
            {
                var position = GeoPosition.GetPositionInMeters(poi.Position, origin);
                if (IsInsideAnyTriangle(position, triList))
                {
                    insidePois.Add(poi);
                }
            }

            var pois = insidePois.Select(pm => poiData.pois.FirstOrDefault(p => p.SlotId == pm.SlotId));
            return pois.ToList();
        }

        private static bool IsInsideAnyTriangle(Vector2 position, List<Vector2[]> triangles)
        {
            foreach (var tries in triangles)
            {
                var polyBound = new PolyBound();
                polyBound.SetPoints(tries.ToList());
                if (polyBound.IsPointInside(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
