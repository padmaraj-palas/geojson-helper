using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonGeometryCollection : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonGeometry[] Geometries { get; set; }
    }
}
