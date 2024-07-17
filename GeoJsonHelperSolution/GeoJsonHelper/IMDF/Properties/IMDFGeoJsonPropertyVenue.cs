using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyVenue : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public string Restriction { get; set; }
        public Label Name { get; set; }
        public Label Alt_name { get; set; }
        public string Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public GeoJsonPoint Display_point { get; set; }
        public Guid? Address_id { get; set; }
    }
}
