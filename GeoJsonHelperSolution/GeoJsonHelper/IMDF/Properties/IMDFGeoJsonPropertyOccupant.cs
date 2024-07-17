using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyOccupant : IMDFGeoJsonProperty
    {
        public Label Name { get; set; }
        public string Category { get; set; }
        public Guid? Anchor_id { get; set; }
        public string Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Temporality Validity { get; set; }
        public Guid? Correlation_id { get; set; }
    }
}
