using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonPoint : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition Coordinates { get; set; }
    }
}
