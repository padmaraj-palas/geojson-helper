using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonMultiPoint : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[] Coordinates { get; set; }
    }
}
