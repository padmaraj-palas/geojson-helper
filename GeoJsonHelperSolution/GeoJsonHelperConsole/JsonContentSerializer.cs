using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace GeoJsonHelperConsole
{
    public static class JsonContentSerializer
    {
        private static readonly JsonSerializer _serializer;

        static JsonContentSerializer()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };

            _serializer.Converters.Add(new StringEnumConverter());
        }

        public static T Deserialize<T>(string json)
        {
            using (TextReader textReader = new StringReader(json))
            {
                using (JsonReader jsonReader = new JsonTextReader(textReader))
                {
                    return _serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public static string Serialize<T>(T value)
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }
    }
}
