using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonMultiPoint : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[] Coordinates { get; set; }
    }
}
