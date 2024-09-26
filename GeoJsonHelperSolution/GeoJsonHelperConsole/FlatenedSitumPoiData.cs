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
        public string X_Data { get; set; }

        public static implicit operator SitumPoiData(FlatenedSitumPoiData poiData)
        {
            return new SitumPoiData
            {
                BuildingId = poiData.BuildingId,
                CategoryName = string.Empty,
                CustomFields = new SitumPoiData.KeyValue[] { new SitumPoiData.KeyValue { Key = "priority", Value = $"{poiData.Priority}" } },
                Id = poiData.Id,
                Name = poiData.Name,
                Position = new SitumPoiData.Pos
                {
                    Latitude = poiData.Latitude,
                    Longitude = poiData.Longitude,
                    FloorId = poiData.FloorId,
                    Georeferences = new SitumPoiData.GeoRef
                    {
                        Latitude = poiData.Latitude,
                        Longitude = poiData.Longitude
                    }
                },
                Type = poiData.Type
            };
        }
    }
}
