using System.Threading.Tasks;
using GeoJsonHelper.GeoJsonObjects;
using GeoJsonHelper.CustomConverters;

namespace GeoJsonHelper
{
    public class GeojsonService : IGeoJsonService
    {
        private GeoJson _geoJson;

        public GeojsonService()
        {
            GeoJsonParser.Init(GetGeoJsonCreatedCallback());
        }

        public GeoJson GeoJson => _geoJson;

        protected virtual IGeoJsonCreatedCallback GetGeoJsonCreatedCallback()
        {
            return null;
        }

        public GeoJson Load(string filepath)
        {
            return _geoJson = GeoJsonParser.Deserialize<GeoJson>(filepath);
        }

        public Task<GeoJson> LoadAsync(string filepath)
        {
            TaskCompletionSource<GeoJson> taskCompletionSource = new TaskCompletionSource<GeoJson>();
            Task.Run(() =>
            {
                var geoJson = Load(filepath);
                taskCompletionSource.SetResult(geoJson);
            });

            return taskCompletionSource.Task;
        }
    }
}
