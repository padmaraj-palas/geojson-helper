using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonLineString : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[] Coordinates { get; set; }
    }
}
