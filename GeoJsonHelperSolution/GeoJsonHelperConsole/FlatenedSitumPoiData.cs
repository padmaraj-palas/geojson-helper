using Newtonsoft.Json;

namespace GeoJsonHelperConsole
{
    public sealed class FlatenedSitumPoiData
    {
        [JsonProperty("Building_id")]
        public int BuildingId { get; set; }

        [JsonProperty("Floor_id")]
        public int FloorId { get; set; }
        public int Id { get; set; }

        [JsonProperty("georeferences.lat")]
        public double Latitude { get; set; }

        [JsonProperty("georeferences.lng")]
        public double Longitude { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public float Radius { get; set; }
        public string Type { get; set; }
        public long X_Data { get; set; }
    }
}
