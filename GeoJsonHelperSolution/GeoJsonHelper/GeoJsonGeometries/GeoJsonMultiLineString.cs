using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonMultiLineString : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonLineString[] Coordinates { get; set; }
    }
}
