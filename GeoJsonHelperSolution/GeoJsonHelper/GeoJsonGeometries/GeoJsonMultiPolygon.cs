namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonMultiPolygon : GeoJsonGeometry
    {
        public GeoJsonPolygon[] Coordinates { get; set; }
    }
}
