using System.IO;
using GeoJsonParser.Apple.CustomConverters;
using GeoJsonParser.Apple.Factories;
using GeoJsonParser.CustomConverters;
using GeoJsonParser.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GeoJsonParser
{
    public static class GeoJsonParser
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
            geoJsonFactory = new AppleGeoJsonObjectFactory(geoJsonFactory);
            _serializer.Converters.Add(new GeoJsonConverter(geoJsonFactory, geoJsonService));
            _serializer.Converters.Add(new LabelConverter());
            _serializer.Converters.Add(new PositionConverter());
        }

        //public static T Serialize<T>(string json)
        //{
        //    using (TextReader textReader = new StringReader(json))
        //    {
        //        using (JsonReader jsonReader = new JsonTextReader(textReader))
        //        {
        //            return _serializer.Deserialize<T>(jsonReader);
        //        }
        //    }
        //}

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
