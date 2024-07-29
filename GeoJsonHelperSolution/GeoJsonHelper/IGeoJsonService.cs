using GeoJsonHelper.GeoJsonObjects;
using System.Threading.Tasks;

namespace GeoJsonHelper
{
    public interface IGeoJsonService
    {
        GeoJson GeoJson { get; }
        GeoJson Load(string filepath);
        Task<GeoJson> LoadAsync(string filepath);
    }
}
