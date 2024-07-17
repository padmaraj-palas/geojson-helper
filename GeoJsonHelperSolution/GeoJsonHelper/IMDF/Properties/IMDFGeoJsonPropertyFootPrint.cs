using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyFootPrint : IMDFGeoJsonProperty
    {
        public string Category { get; set; }
        public Label Name { get; set; }
        public Guid[] Building_ids { get; set; }
    }
}
