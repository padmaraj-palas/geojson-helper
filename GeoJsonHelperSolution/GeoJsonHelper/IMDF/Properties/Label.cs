using System.Collections.Generic;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class Label
    {
        public Dictionary<string, string> Elements { get; set; }

        public static implicit operator Label(Dictionary<string, string> dict)
        {
            return new Label { Elements = dict };
        }

        public static implicit operator Dictionary<string, string>(Label label)
        {
            return label == null ? null : label.Elements;
        }
    }
}
