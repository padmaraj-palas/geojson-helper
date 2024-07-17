namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonMultiLineString : GeoJsonGeometry
    {
        public GeoJsonLineString[] Coordinates { get; set; }
    }
}
