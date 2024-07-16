using GeoJsonParser.GeoJsonObjects;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.GeoJsonFeatures
{
    public class AppleGeoJsonFeature : GeoJsonFeature
    {
        public FeatureTypes Feature_type { get; set; }
    }

    public sealed class AppleGeoJsonFeature<T> : AppleGeoJsonFeature
    {
        [MaybeNull] public T Properties { get; set; }
    }
}
