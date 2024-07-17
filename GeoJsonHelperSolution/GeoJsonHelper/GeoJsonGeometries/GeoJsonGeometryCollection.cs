namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonGeometryCollection : GeoJsonGeometry
    {
        public GeoJsonGeometry[] Geometries { get; set; }
    }
}
