using GeoJsonHelper.GeoJsonObjects;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.GeoJsonFeatures
{
    public class IMDFGeoJsonFeature : GeoJsonFeature
    {
        public FeatureTypes Feature_type { get; set; }
    }

    public sealed class IMDFGeoJsonFeature<T> : IMDFGeoJsonFeature
    {
        [MaybeNull] public T Properties { get; set; }
    }
}
