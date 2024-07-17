namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyAddress : IMDFGeoJsonProperty
    {
        public string Address { get; set; }
        public string Unit { get; set; }
        public string Locality { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Postal_code { get; set; }
        public string Postal_code_ext { get; set; }
        public string Postal_code_vanity { get; set; }
    }
}
