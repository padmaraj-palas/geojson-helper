using Newtonsoft.Json;

namespace GeoJsonHelperConsole
{
    public sealed class PoiData
    {
        [JsonProperty("Building_id")]
        public int BuildingId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Pos Position { get; set; }
        public string Type { get; set; }

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
    }
}
