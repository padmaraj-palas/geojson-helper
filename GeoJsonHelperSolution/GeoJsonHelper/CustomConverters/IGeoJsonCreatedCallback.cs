using GeoJsonHelper.GeoJsonObjects;

namespace GeoJsonHelper.CustomConverters
{
    public interface IGeoJsonCreatedCallback
    {
        void OnGeoJsonCreated(GeoJson geoJson);
    }
}
