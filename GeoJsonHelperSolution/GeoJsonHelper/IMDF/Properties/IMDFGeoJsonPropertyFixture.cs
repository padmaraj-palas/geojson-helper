using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyFixture : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public Label Name { get; set; }
        public Label Alt_Name { get; set; }
        public Guid? Anchor_id { get; set; }
        public Guid? Level_id { get; set; }
        public GeoJsonPoint Display_point { get; set; }
    }
}
