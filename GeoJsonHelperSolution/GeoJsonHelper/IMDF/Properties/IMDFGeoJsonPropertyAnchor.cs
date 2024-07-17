using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyAnchor : IMDFGeoJsonProperty
    {
        public Guid? Address_id { get; set; }
        public Guid? Unit_id { get; set; }
    }
}
