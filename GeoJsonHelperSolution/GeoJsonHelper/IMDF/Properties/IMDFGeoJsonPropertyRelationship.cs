using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyRelationship : IMDFGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public string Direction { get; set; }
        [MaybeNull] public FeatureReference? Origin { get; set; }
        [MaybeNull] public FeatureReference[] Intermediary { get; set; }
        [MaybeNull] public FeatureReference? Destination { get; set; }
        [MaybeNull] public string Hours { get; set; }
    }
}
