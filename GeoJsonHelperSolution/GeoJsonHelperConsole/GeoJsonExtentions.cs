using GeoJsonHelper.GeoJsonGeometries;
using GeoJsonHelper.GeoJsonObjects;
using GeoPositioning;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoJsonExtentions
    {
        public static (Vector2 min, Vector2 max, Vector2 size) GetBounds(this GeoJsonFeature feature, GeoPosition origin)
        {
            Vector2 max = new Vector2(double.MinValue, double.MinValue);
            Vector2 min = new Vector2(double.MaxValue, double.MaxValue);
            var positions = feature.GetPositionsFromFeature(origin);
            foreach (var position in positions)
            {
                if (position.X < min.X)
                {
                    min.X = position.X;
                }

                if (position.X > max.X)
                {
                    max.X = position.X;
                }

                if (position.Y < min.Y)
                {
                    min.Y = position.Y;
                }

                if (position.Y > max.Y)
                {
                    max.Y = position.Y;
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

            var polyIndex = 0;

            var positions = new Vector2[polygon.Coordinates[polyIndex].LineString.Coordinates.Length];
            if (positions.Length <= 2)
            {
                return null;
            }

            for (int i = 0; i < polygon.Coordinates[polyIndex].LineString.Coordinates.Length; i++)
            {
                var pos = polygon.Coordinates[polyIndex].LineString.Coordinates[i];
                GeoPosition geoPosition = new GeoPosition((double)pos.Latitude, (double)pos.Longitude);
                positions[i] = GeoPosition.GetPositionInMeters(geoPosition, origin);
            }

            return positions;
        }
    }
}
