namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonPolygon : GeoJsonGeometry
    {
        public GeoJsonLineRing[] Coordinates { get; set; }
    }
}
