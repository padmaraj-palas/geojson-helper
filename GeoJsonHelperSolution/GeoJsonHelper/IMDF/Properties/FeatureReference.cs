using System;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class FeatureReference
    {
        public Guid? Id { get; set; }
        public FeatureTypes Feature_type { get; set; }
    }
}
