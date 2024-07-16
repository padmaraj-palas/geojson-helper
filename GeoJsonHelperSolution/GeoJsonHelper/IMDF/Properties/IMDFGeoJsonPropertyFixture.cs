using GeoJsonHelper.GeoJsonGeometries;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyFixture : IMDFGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Alt_Name { get; set; }
        [MaybeNull] public Guid? Anchor_id { get; set; }
        [MaybeNull] public Guid? Level_id { get; set; }
        [MaybeNull] public GeoJsonPoint Display_point { get; set; }
    }
}
