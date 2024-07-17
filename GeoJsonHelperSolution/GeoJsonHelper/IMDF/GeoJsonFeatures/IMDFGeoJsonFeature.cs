using GeoJsonHelper.GeoJsonObjects;
using GeoJsonHelper.IMDF.Properties;


namespace GeoJsonHelper.IMDF.GeoJsonFeatures
{
    public class IMDFGeoJsonFeature : GeoJsonFeature
    {
        public FeatureTypes Feature_type { get; set; }
    }

    public sealed class IMDFGeoJsonFeature<T> : IMDFGeoJsonFeature
        where T : IMDFGeoJsonProperty
    {
        public T Properties { get; set; }
    }
}
