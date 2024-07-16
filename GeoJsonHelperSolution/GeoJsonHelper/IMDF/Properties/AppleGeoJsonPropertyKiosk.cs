using GeoJsonParser.GeoJsonGeometries;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class AppleGeoJsonPropertyKiosk : AppleGeoJsonProperty
    {
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Alt_name { get; set; }
        [MaybeNull] public Guid? Anchor_id { get; set; }
        [MaybeNull] public Guid? Level_id { get; set; }
        [MaybeNull] public GeoJsonPoint Display_point { get; set; }
    }
}
