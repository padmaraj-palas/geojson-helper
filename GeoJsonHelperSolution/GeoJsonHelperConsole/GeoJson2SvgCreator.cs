using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoJson2SvgCreator
    {
        public static void Create(Vector2[] rootVertices, IList<(Vector2[] vertices, PoiData poi, string groupName)> mapping)
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
            svgDocument.ViewBox = new SvgViewBox((float)min.X, (float)min.Y, (float)size.X, (float)size.Y);
            SvgRectangle svgRectangle = new SvgRectangle();
            svgRectangle.X = (float)min.X;
            svgRectangle.Y = (float)min.Y;
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

        public static void DrawFeature(SvgDocument svgRoot, (Vector2[] vertices, PoiData poi, string groupName) mapping)
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

            var svgText = new SvgText($"{mapping.poi.Id}");
            svgText.Stroke = SvgPaintServer.None;
            svgText.Fill = new SvgColourServer(Color.FromArgb((byte)(color.R * 0.75f), (byte)(color.G * 0.75f), (byte)(color.B * 0.75f)));
            svgText.TextAnchor = SvgTextAnchor.Middle;
            svgText.X = x;
            svgText.Y = y;
            svgText.FontSize = new SvgUnit((float)Math.Min(svgPolygon.Bounds.Width, svgPolygon.Bounds.Height) / 10f);
            poiRoot.Children.Add(svgText);
        }
    }
}
