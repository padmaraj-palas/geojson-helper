using GeoJsonHelper.GeoJsonGeometries;
using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyLevel : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public string Restriction { get; set; }
        public bool Outdoor { get; set; }
        public int Ordinal { get; set; }
        public Label Name { get; set; }
        public Label Short_name { get; set; }
        public GeoJsonPoint Display_point { get; set; }
        public Guid? Address_id { get; set; }
        public Guid[] Building_ids { get; set; }
    }
}
