using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonMultiPolygon : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPolygon[] Coordinates { get; set; }
    }
}
