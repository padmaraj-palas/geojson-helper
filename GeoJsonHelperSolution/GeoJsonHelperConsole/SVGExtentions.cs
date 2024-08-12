using Svg;
using Svg.Transforms;
using System.Drawing;
using System.Linq;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class SVGExtentions
    {
        public static SvgPolygon ToSvgPolygon(this Vector2[] positions, Color? fillColor = null, Vector2 position = default, SvgElement root = null, Vector2? scale = null)
        {
            var svgPolygon = new SvgPolygon();
            svgPolygon.Stroke = SvgPaintServer.None;
            if (fillColor != null)
            {
                svgPolygon.Fill = new SvgColourServer(fillColor.Value);
            }

            SvgPointCollection svgUnits = new SvgPointCollection();
            svgUnits.AddRange(positions.Select(p => p - position).SelectMany(p => new SvgUnit[] { new SvgUnit((float)p.X), new SvgUnit(-(float)p.Y) }));
            svgPolygon.Points = svgUnits;

            scale = scale != null ? scale.Value : Vector2.One;
            SvgTransformCollection svgTransforms = new SvgTransformCollection
            {
                new SvgTranslate((float)position.X, -(float)position.Y),
                new SvgScale((float)scale.Value.X, (float)scale.Value.Y)
            };
            svgPolygon.Transforms = svgTransforms;

            if (root != null)
            {
                root.Children.Add(svgPolygon);
            }

            return svgPolygon;
        }

        public static SvgText CreateSvgText(string text, Color? fillColor = null, float fontSize = 24, Vector2 position = default, SvgElement root = null)
        {
            SvgUnitCollection x = new SvgUnitCollection
            {
                (float) position.X
            };

            SvgUnitCollection y = new SvgUnitCollection
            {
                (float) position.Y
            };

            fillColor = fillColor != null ? fillColor.Value : Color.White;

            var svgText = new SvgText(text);
            svgText.Stroke = SvgPaintServer.None;
            svgText.Fill = new SvgColourServer(Color.FromArgb((byte)(fillColor.Value.R * 0.75f), (byte)(fillColor.Value.G * 0.75f), (byte)(fillColor.Value.B * 0.75f)));
            svgText.TextAnchor = SvgTextAnchor.Middle;
            svgText.X = x;
            svgText.Y = y;
            svgText.FontSize = new SvgUnit(fontSize);

            if (root != null)
            {
                root.Children.Add(svgText);
            }

            return svgText;
        }
    }
}
