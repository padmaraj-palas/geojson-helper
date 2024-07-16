using System;
using GeoJsonParser.GeoJsonGeometries;
using Newtonsoft.Json;

namespace GeoJsonParser.CustomConverters
{
    public sealed class PositionConverter : JsonConverter<GeoJsonPosition>
    {
        public override GeoJsonPosition? ReadJson(JsonReader reader, Type objectType, GeoJsonPosition? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                var path = reader.Path;
                return serializer.Deserialize<decimal[]>(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, GeoJsonPosition? value, JsonSerializer serializer)
        {
            try
            {
                serializer.Serialize(writer, (decimal[])value);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}
