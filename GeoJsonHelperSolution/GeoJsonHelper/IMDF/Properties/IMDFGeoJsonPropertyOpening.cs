using GeoJsonHelper.GeoJsonGeometries;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyOpening : IMDFGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public string[] Accessibility { get; set; }
        [MaybeNull] public string[] Access_control { get; set; }
        [MaybeNull] public Door Door { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Alt_name { get; set; }
        [MaybeNull] public GeoJsonPoint Display_point { get; set; }
        [MaybeNull] public Guid? Level_id { get; set; }
    }
}
