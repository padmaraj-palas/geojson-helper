using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonPolygon : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonLineRing[] Coordinates { get; set; }
    }
}
