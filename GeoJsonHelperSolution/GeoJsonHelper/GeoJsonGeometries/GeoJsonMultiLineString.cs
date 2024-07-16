using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonMultiLineString : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonLineString[] Coordinates { get; set; }
    }
}
