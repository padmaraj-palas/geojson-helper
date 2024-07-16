using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonLineString : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[] Coordinates { get; set; }
    }
}
