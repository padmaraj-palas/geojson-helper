using System;
using System.Drawing;

namespace GeoJsonHelperConsole
{
    public static class ColorUtility
    {
        private static Random random = new Random(1);

        public static Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(50, 255), random.Next(50, 255), random.Next(50, 255));
        }
    }
}
