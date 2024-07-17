namespace GeoJsonHelper.GeoJsonObjects
{
    public abstract class GeoJson
    {
        public decimal[] Bbox { get; set; }
        public GeoJsonObjectTypes Type { get; set; }
    }
}
