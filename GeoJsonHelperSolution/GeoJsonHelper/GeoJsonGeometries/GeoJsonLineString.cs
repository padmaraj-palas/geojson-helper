using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonLineString : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[] Coordinates { get; set; }

        public static implicit operator GeoJsonLineString(GeoJsonPosition[] positions)
        {
            if (positions == null || positions.Length == 0)
            {
                return null;
            }

            return new GeoJsonLineString { Coordinates = positions, Type = GeoJsonObjectTypes.LineString };
        }

        public static implicit operator GeoJsonPosition[](GeoJsonLineString lineString)
        {
            return lineString?.Coordinates;
        }
    }
}
