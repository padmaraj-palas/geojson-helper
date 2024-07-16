using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonMultiPolygon : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPolygon[] Coordinates { get; set; }
    }
}
