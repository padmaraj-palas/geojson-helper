using GeoJsonParser.GeoJsonGeometries;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyBuilding : AppleGeoJsonProperty
    {
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Alt_name { get; set; }
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public string Restriction { get; set; }
        [MaybeNull] public GeoJsonPoint Display_point { get; set; }
        [MaybeNull] public Guid? Address_id { get; set; }
    }
}
