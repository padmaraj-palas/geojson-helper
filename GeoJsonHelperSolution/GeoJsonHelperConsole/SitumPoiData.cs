using Newtonsoft.Json;
using System.Drawing;

namespace GeoJsonHelperConsole
{
    public sealed class SitumPoiData
    {
        [JsonProperty("Building_id")]
        public int BuildingId { get; set; }
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
        [JsonProperty("custom_fields")]
        public KeyValue[] CustomFields { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Pos Position { get; set; }
        public string Type { get; set; }

        [JsonIgnore]
        public Color FillColor { get; set; }

        public sealed class Pos
        {
            [JsonProperty("Floor_id")]
            public int FloorId { get; set; }

            [JsonProperty("Lat")]
            public double Latitude { get; set; }

            [JsonProperty("Lng")]
            public double Longitude { get; set; }

            public GeoRef Georeferences { get; set; }
        }

        public sealed class GeoRef
        {
            [JsonProperty("Lat")]
            public double Latitude { get; set; }

            [JsonProperty("Lng")]
            public double Longitude { get; set; }
        }

        public sealed class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
