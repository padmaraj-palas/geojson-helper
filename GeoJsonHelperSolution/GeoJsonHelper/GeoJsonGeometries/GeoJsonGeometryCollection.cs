using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonGeometryCollection : GeoJsonGeometry
    {
        [MaybeNull] public GeoJsonGeometry[] Geometries { get; set; }
    }
}
