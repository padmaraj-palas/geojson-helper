using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyBuilding : IMDFGeoJsonProperty
    {
        public Label Name { get; set; }
        public Label Alt_name { get; set; }
        public string Category { get; set; }
        public string Restriction { get; set; }
        public GeoJsonPoint Display_point { get; set; }
        public Guid? Address_id { get; set; }
    }
}
