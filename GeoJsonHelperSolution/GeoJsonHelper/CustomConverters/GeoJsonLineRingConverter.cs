using System;
using GeoJsonHelper.GeoJsonGeometries;
using Newtonsoft.Json;

namespace GeoJsonHelper.CustomConverters
{
    internal sealed class GeoJsonLineRingConverter : JsonConverter<GeoJsonLineRing>
    {
        public override GeoJsonLineRing ReadJson(JsonReader reader, Type objectType, GeoJsonLineRing existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (GeoJsonLineString)serializer.Deserialize<GeoJsonPosition[]>(reader);
        }

        public override void WriteJson(JsonWriter writer, GeoJsonLineRing value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (GeoJsonPosition[])(GeoJsonLineString)value);
        }
    }
}
