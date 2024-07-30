using GeoPositioning;

namespace GeoJsonHelperConsole
{
    public sealed class PoiMetaData
    {
        public GeoPosition Position { get; set; }
        public int Priority { get; set; }
        public int SlotId { get; set; }
    }
}
