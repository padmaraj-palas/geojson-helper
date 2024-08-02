using System;
using System.Collections.Generic;
using System.Drawing;

namespace GeoJsonHelperConsole
{
    public static class ColorUtility
    {
        private static readonly IDictionary<string, Color> colors;
        private static readonly Random random;

        static ColorUtility()
        {
            colors = new Dictionary<string, Color>();
            random = new Random(8);
        }

        public static Color GetRandomColor(string key = "")
        {
            Color color;
            if (string.IsNullOrEmpty(key) || !colors.TryGetValue(key, out color))
            {
                color = Color.FromArgb(random.Next(150, 255), random.Next(150, 255), random.Next(0, 225));
                if (!string.IsNullOrEmpty(key))
                {
                    colors.Add(key, color);
                }
            }

            return color;
        }
    }
}
