using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyAmenity : AppleGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public string Accessibility { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Alt_name { get; set; }
        [MaybeNull] public string Hours { get; set; }
        [MaybeNull] public string Phone { get; set; }
        [MaybeNull] public string Website { get; set; }
        [MaybeNull] public Guid[] Unit_ids { get; set; }
        [MaybeNull] public Guid? Address_id { get; set; }
        [MaybeNull] public Guid? Correlation_id { get; set; }
    }
}
