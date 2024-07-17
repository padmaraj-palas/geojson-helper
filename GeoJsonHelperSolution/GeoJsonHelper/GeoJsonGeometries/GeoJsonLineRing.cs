namespace GeoJsonHelper.GeoJsonGeometries
{
    public sealed class GeoJsonLineRing
    {
        public GeoJsonLineString LineString { get; set; }

        public override string ToString()
        {
            return LineString?.Coordinates?.ToString();
        }

        public static implicit operator GeoJsonLineRing(GeoJsonLineString lineString)
        {
            return lineString == null ? null : new GeoJsonLineRing { LineString = lineString };
        }

        public static implicit operator GeoJsonLineString(GeoJsonLineRing lineRing)
        {
            return lineRing?.LineString;
        }
    }
}
