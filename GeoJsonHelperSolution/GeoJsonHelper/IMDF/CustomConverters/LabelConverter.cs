using System;
using System.Collections.Generic;
using GeoJsonHelper.IMDF.Properties;
using Newtonsoft.Json;

namespace GeoJsonHelper.IMDF.CustomConverters
{
    internal sealed class LabelConverter : JsonConverter<Label>
    {
        public override Label ReadJson(JsonReader reader, Type objectType, Label existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<Dictionary<string, string>>(reader);
        }

        public override void WriteJson(JsonWriter writer, Label value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (Dictionary<string, string>)value);
        }
    }
}
