namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyRelationship : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public string Direction { get; set; }
        public FeatureReference Origin { get; set; }
        public FeatureReference[] Intermediary { get; set; }
        public FeatureReference Destination { get; set; }
        public string Hours { get; set; }
    }
}
