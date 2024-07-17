using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertySection : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public string Restriction { get; set; }
        public string[] Accessibility { get; set; }
        public Label Name { get; set; }
        public Label Alt_name { get; set; }
        public GeoJsonPoint Display_point { get; set; }
        public Guid? Level_id { get; set; }
        public Guid? Address_id { get; set; }
        public Guid? Correlation_id { get; set; }
        public Guid[] Parents { get; set; }
    }
}
