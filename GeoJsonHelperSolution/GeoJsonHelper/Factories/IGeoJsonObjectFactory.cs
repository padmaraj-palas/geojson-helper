using GeoJsonParser.GeoJsonObjects;
using Newtonsoft.Json.Linq;

namespace GeoJsonParser.Factories
{
    public interface IGeoJsonObjectFactory
    {
        GeoJson? CreateGeoJson(JObject jObject, GeoJsonObjectTypes objectType);
    }
}
