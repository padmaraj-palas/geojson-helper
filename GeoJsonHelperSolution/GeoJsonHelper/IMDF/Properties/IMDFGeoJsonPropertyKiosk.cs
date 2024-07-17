using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyKiosk : IMDFGeoJsonProperty
    {
        public Label Name { get; set; }
        public Label Alt_name { get; set; }
        public Guid? Anchor_id { get; set; }
        public Guid? Level_id { get; set; }
        public GeoJsonPoint Display_point { get; set; }
    }
}
