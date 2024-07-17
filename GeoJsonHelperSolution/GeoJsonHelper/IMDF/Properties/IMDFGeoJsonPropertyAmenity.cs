using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyAmenity : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public string Accessibility { get; set; }
        public Label Name { get; set; }
        public Label Alt_name { get; set; }
        public string Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Guid[] Unit_ids { get; set; }
        public Guid? Address_id { get; set; }
        public Guid? Correlation_id { get; set; }
    }
}
