using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace GeoJsonHelperConsole
{
    internal static class PoiJsonSerializer
    {
        private static readonly JsonSerializer _serializer;

        static PoiJsonSerializer()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        public static T Deserialize<T>(string json)
        {
            using (var reader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return _serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
