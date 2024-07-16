using System;

namespace GeoJsonParser.Apple.Properties
{
    public sealed class Temporality
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Modified { get; set; }
    }
}
