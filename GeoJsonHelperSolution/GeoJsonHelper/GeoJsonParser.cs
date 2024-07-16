using System.IO;
using GeoJsonHelper.CustomConverters;
using GeoJsonHelper.IMDF.CustomConverters;
using GeoJsonHelper.IMDF.Factories;
using GeoJsonHelper.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GeoJsonHelper
{
    internal static class GeoJsonParser
    {
        private static readonly JsonSerializer _serializer;

        static GeoJsonParser()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        public static void Init(IGeoJsonService geoJsonService)
        {
            IGeoJsonObjectFactory geoJsonFactory = new GeoJsonObjectFactory();
            geoJsonFactory = new IMDFGeoJsonObjectFactory(geoJsonFactory);
            _serializer.Converters.Add(new GeoJsonConverter(geoJsonFactory, geoJsonService));
            _serializer.Converters.Add(new GeoJsonLineRingConverter());
            _serializer.Converters.Add(new LabelConverter());
            _serializer.Converters.Add(new PositionConverter());
        }

        public static T Serialize<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using (TextReader textReader = new StreamReader(fileStream))
                {
                    using (JsonReader jsonReader = new JsonTextReader(textReader))
                    {
                        return _serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }
    }
}
