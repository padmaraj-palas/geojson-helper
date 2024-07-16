using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonPolygon : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonPosition[][] Coordinates { get; set; }
    }
}
