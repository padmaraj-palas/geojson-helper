using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyAddress : AppleGeoJsonProperty
    {
        [MaybeNull] public string Address { get; set; }
        [MaybeNull] public string Unit { get; set; }
        [MaybeNull] public string Locality { get; set; }
        [MaybeNull] public string Province { get; set; }
        [MaybeNull] public string Country { get; set; }
        [MaybeNull] public string Postal_code { get; set; }
        [MaybeNull] public string Postal_code_ext { get; set; }
        [MaybeNull] public string Postal_code_vanity { get; set; }
    }
}
