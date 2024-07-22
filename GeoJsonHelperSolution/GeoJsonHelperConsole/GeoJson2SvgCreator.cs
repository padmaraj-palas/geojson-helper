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

namespace GeoJsonHelperConsole
{
    public static class GeoJson2SvgCreator
    {
        public static void Create(GeoJsonFeature rootFeature, GeoPosition origin, IList<(GeoJsonFeature feature, PoiData poi, string groupName)> mapping)
        {
            SvgDocument svgDocument = new();
            CreateRoot(svgDocument, rootFeature, origin);

            foreach (var item in mapping)
            {
                DrawFeature(svgDocument, item, origin);
            }

            svgDocument.Write(Path.GetFullPath("./results.svg"));
        }

        private static void CreateRoot(SvgDocument svgDocument, GeoJsonFeature rootFeature, GeoPosition origin)
        {
            var (min, max, size) = rootFeature.GetBounds(origin);
            svgDocument.ViewBox = new SvgViewBox((float)min.X, (float)min.Y, (float)size.X, (float)size.Y);
            SvgRectangle svgRectangle = new SvgRectangle();
            svgRectangle.X = (float)min.X;
            svgRectangle.Y = (float)min.Y;
            svgRectangle.Width = (float)size.X;
            svgRectangle.Height = (float)size.Y;
            svgRectangle.Stroke = SvgPaintServer.None;
            svgRectangle.Fill = new SvgColourServer(Color.Black);
            svgDocument.Children.Add(svgRectangle);
        }

        public static void DrawFeature(SvgDocument svgRoot, (GeoJsonFeature feature, PoiData poi, string groupName) mapping, GeoPosition origin)
        {
            var positions = mapping.feature.GetPositionsFromFeature(origin);
            if (positions == null)
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
            svgUnits.AddRange(positions.SelectMany(p => new SvgUnit[] { new SvgUnit((float)p.X), new SvgUnit(-(float)p.Y) }));
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
