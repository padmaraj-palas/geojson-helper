using GeoJsonHelper.GeoJsonObjects;
using Newtonsoft.Json.Linq;

namespace GeoJsonHelper.Factories
{
    internal interface IGeoJsonObjectFactory
    {
        GeoJson CreateGeoJson(JObject jObject, GeoJsonObjectTypes objectType);
    }
}
