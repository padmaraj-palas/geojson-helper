using System;
using GeoJsonHelper.Factories;
using GeoJsonHelper.GeoJsonObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoJsonHelper.CustomConverters
{
    internal sealed class GeoJsonConverter : JsonConverter<GeoJson>
    {
        private readonly IGeoJsonCreatedCallback _geoJsonCreatedCallback;
        private readonly IGeoJsonObjectFactory _geoJsonObjectFactory;

        public GeoJsonConverter(IGeoJsonObjectFactory geoJsonObjectFactory, IGeoJsonCreatedCallback geoJsonCreatedCallback)
        {
            _geoJsonObjectFactory = geoJsonObjectFactory;
            _geoJsonCreatedCallback = geoJsonCreatedCallback;
        }

        public override bool CanWrite => false;

        public override GeoJson ReadJson(JsonReader reader, Type objectType, GeoJson existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                JObject jObject = JObject.Load(reader);
                JToken geoJsonType;
                if (!jObject.TryGetValue("type", out geoJsonType))
                {
                    throw new ArgumentException("GeoJsonConverter: GeoJson type not specified");
                }

                var type = Enum.Parse<GeoJsonObjectTypes>(geoJsonType.ToString(), true);
                var value = Parse(jObject, type, serializer);
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, GeoJson value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private GeoJson Parse(JObject jObject, GeoJsonObjectTypes type, JsonSerializer serializer)
        {
            GeoJson geoJson = _geoJsonObjectFactory.CreateGeoJson(jObject, type);

            if (geoJson != null)
            {
                serializer.Populate(jObject.CreateReader(), geoJson);
                _geoJsonCreatedCallback?.OnGeoJsonCreated(geoJson);
            }

            return geoJson;
        }
    }
}
