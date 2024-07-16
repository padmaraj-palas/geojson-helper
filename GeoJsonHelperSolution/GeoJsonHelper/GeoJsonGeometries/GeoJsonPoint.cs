using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonPoint : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition Coordinates { get; set; }
    }
}
