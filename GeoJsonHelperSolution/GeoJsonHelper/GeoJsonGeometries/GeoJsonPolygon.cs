using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonPolygon : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[][] Coordinates { get; set; }
    }
}
