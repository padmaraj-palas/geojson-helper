using CSVHelper;
using GeoJsonHelper;
using GeoJsonHelper.GeoJsonObjects;
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

            if (records == null || records.Length == 0)
            {
                return;
            }

            var json = records.ToJson();
            var pois = PoiJsonSerializer.Deserialize<FlatenedSitumPoiData[]>(json).Select(fp => (SitumPoiData)fp).Where(p => p.Position.FloorId == 109);

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

            //var mapping = GeoJsonAndSitumPoiMapping.GenerateMockPoisForGeoJson(units, origin);
            var mapping = await GeoJsonAndSitumPoiMapping.MapGeoJsonAndPoiAsync(units, pois, origin);
            Create(geoJsonService.Venues.Values.FirstOrDefault(), mapping.matches.Where(kvp => kvp.Key is GeoJsonFeature).Select(kvp => new KeyValuePair<GeoJsonFeature, SitumPoiData>(kvp.Key, kvp.Value)), origin);

            var (min, max, size) = geoJsonService.Venues.Values.FirstOrDefault().GetVerticesFromFeature(origin)[0].GetBounds();
            var lowerLeftCorner = min;
            var upperLeftCorner = lowerLeftCorner + Vector2.Up * size.Y;
            var upperRightCorner = max;
            var lowerRightCorner = lowerLeftCorner + Vector2.Right * size.X;

            var angle = Vector2.Angle(Vector2.Up, Vector2.Right);

            var lf = GeoPosition.GetGeoPositionFromMetricPosition(origin, lowerLeftCorner);
            var ul = GeoPosition.GetGeoPositionFromMetricPosition(origin, upperLeftCorner);
            var ur = GeoPosition.GetGeoPositionFromMetricPosition(origin, upperRightCorner);
            var lr = GeoPosition.GetGeoPositionFromMetricPosition(origin, lowerRightCorner);

            var totalMatched = mapping.matches.Values.ToList();
            var totalMultiMatches = mapping.multiMatch.SelectMany(kvp => kvp.Value).ToList();
            var noMatches = mapping.notMatched;
        }

        public static void Create(GeoJsonFeature rootObject, IEnumerable<KeyValuePair<GeoJsonFeature, SitumPoiData>> mapping, GeoPosition origin)
        {
            SvgDocument svgDocument = new();
            var rootVertices = rootObject.GetVerticesFromFeature(origin)[0];
            CreateRoot(svgDocument, rootVertices);

            foreach (var item in mapping)
            {
                DrawFeature(svgDocument, item, origin);
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

        public static void DrawFeature(SvgDocument svgRoot, KeyValuePair<GeoJsonFeature, SitumPoiData> mapping, GeoPosition origin)
        {
            var vertices = mapping.Key.GetVerticesFromFeature(origin)[0];

            if (vertices == null || vertices.Length == 0)
            {
                return;
            }

            var bounds = vertices.GetBounds();
            var center = new Vector2(bounds.min.X + bounds.size.X / 2, bounds.min.Y + bounds.size.Y / 2);
            var maxSize = (float)Math.Max(bounds.size.X, bounds.size.Y);

            var contentRoot = svgRoot.GetElementById(mapping.Value.CategoryName);
            if (contentRoot == null)
            {
                contentRoot = new SvgGroup();
                contentRoot.SetAndForceUniqueID(mapping.Value.CategoryName);
                svgRoot.Children.Add(contentRoot);
            }

            var poiRoot = new SvgGroup();
            poiRoot.SetAndForceUniqueID($"{mapping.Value.Id}");
            contentRoot.Children.Add(poiRoot);

            Color color = mapping.Value.FillColor;
            var scale = new Vector2(1f - (0.1f / maxSize), 1f - (0.1f / maxSize));
            var svgPolygon = vertices.ToSvgPolygon(fillColor: color, position: center, root: poiRoot, scale: scale);
            //poiRoot.Children.Add(svgPolygon);

            //SvgUnitCollection x = new SvgUnitCollection
            //{
            //    svgPolygon.Bounds.X + svgPolygon.Bounds.Width / 2f
            //};

            //SvgUnitCollection y = new SvgUnitCollection
            //{
            //    svgPolygon.Bounds.Y + svgPolygon.Bounds.Height / 2f
            //};

            var textPosition = new Vector2(svgPolygon.Bounds.X + svgPolygon.Bounds.Width / 2f, svgPolygon.Bounds.Y + svgPolygon.Bounds.Height / 2f);
            var textColor = Color.FromArgb((byte)(color.R * 0.75f), (byte)(color.G * 0.75f), (byte)(color.B * 0.75f));
            var fontSize = (float)Math.Min(svgPolygon.Bounds.Width, svgPolygon.Bounds.Height) / 10f;
            var svgText = SVGExtentions.CreateSvgText(mapping.Value.Name, fillColor: textColor, fontSize: fontSize, position: textPosition, root: poiRoot);//new SvgText($"{mapping.Value.Name}");
            //svgText.Stroke = SvgPaintServer.None;
            //svgText.Fill = new SvgColourServer(Color.FromArgb((byte)(color.R * 0.75f), (byte)(color.G * 0.75f), (byte)(color.B * 0.75f)));
            //svgText.TextAnchor = SvgTextAnchor.Middle;
            //svgText.X = x;
            //svgText.Y = y;
            //svgText.FontSize = new SvgUnit((float)Math.Min(svgPolygon.Bounds.Width, svgPolygon.Bounds.Height) / 10f);
            //poiRoot.Children.Add(svgText);
        }
    }
}
