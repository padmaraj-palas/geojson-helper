using GeoJsonHelper.GeoJsonGeometries;
using GeoJsonHelper.GeoJsonObjects;
using GeoPositioning;
using System.Collections.Generic;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoJsonExtentions
    {
        public static (Vector2 min, Vector2 max, Vector2 size) GetBounds(this GeoJsonFeature feature, GeoPosition origin)
        {
            return feature.GetPositionsFromFeature(origin).GetBounds();
        }

        public static (Vector2 min, Vector2 max, Vector2 size) GetBounds(this Vector2[] vertices)
        {
            Vector2 max = new Vector2(double.MinValue, double.MinValue);
            Vector2 min = new Vector2(double.MaxValue, double.MaxValue);
            foreach (var vertice in vertices)
            {
                if (vertice.X < min.X)
                {
                    min.X = vertice.X;
                }

                if (vertice.X > max.X)
                {
                    max.X = vertice.X;
                }

                if (vertice.Y < min.Y)
                {
                    min.Y = vertice.Y;
                }

                if (vertice.Y > max.Y)
                {
                    max.Y = vertice.Y;
                }
            }

            var width = max.X - min.X;
            var height = max.Y - min.Y;

            return (min, max, new Vector2(width, height));
        }

        public static Vector2[] GetPositionsFromFeature(this GeoJsonFeature feature, GeoPosition origin)
        {
            if (feature == null || feature.Geometry == null || feature.Geometry is not GeoJsonPolygon)
            {
                return null;
            }

            var polygon = feature.Geometry as GeoJsonPolygon;
            if (polygon.Coordinates == null || polygon.Coordinates.Length == 0)
            {
                return null;
            }

            var positions = new List<Vector2>();
            for (int polyIndex = 0; polyIndex < polygon.Coordinates.Length; polyIndex++)
            {
                if (polygon.Coordinates[polyIndex].LineString.Coordinates.Length <= 2)
                {
                    return null;
                }

                for (int i = 0; i < polygon.Coordinates[polyIndex].LineString.Coordinates.Length; i++)
                {
                    var pos = polygon.Coordinates[polyIndex].LineString.Coordinates[i];
                    GeoPosition geoPosition = new GeoPosition((double)pos.Latitude, (double)pos.Longitude);
                    positions.Add(GeoPosition.GetPositionInMeters(geoPosition, origin));
                }
            }

            return positions.ToArray();
        }
    }
}
