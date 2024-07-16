using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyFootPrint : AppleGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Guid[] Building_ids { get; set; }
    }
}
