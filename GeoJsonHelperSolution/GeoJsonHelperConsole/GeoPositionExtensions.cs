using GeoPositioning;
using VectorMath;

namespace GeoJsonHelperConsole
{
    public static class GeoPositionExtensions
    {
        public static Vector2[] ToMetric(GeoPosition[] geoPositions, GeoPosition origin)
        {
            Vector2[] positions = new Vector2[geoPositions.Length];
            for (int i = 0; i < geoPositions.Length; i++)
            {
                positions[i] = GeoPosition.GetPositionInMeters(geoPositions[i], origin);
            }

            return positions;
        }
    }
}
