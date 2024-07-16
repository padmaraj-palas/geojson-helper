using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyAnchor : AppleGeoJsonProperty
    {
        [MaybeNull] public Guid? Address_id { get; set; }
        [MaybeNull] public Guid? Unit_id { get; set; }
    }
}
