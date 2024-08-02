using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CSVHelper;

namespace GeoJsonHelperConsole
{
    public static class PoiExporter
    {
        private static readonly JsonSerializer _serializer;

        static PoiExporter()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        public static async Task Export(string geoJsonPath, string poisPath, string outputPath)
        {
            var records = await CSVParser.ParseAsync(poisPath);
            var json = records.ToJson();
            var situmPois = PoiJsonSerializer.Deserialize<SitumPoiData[]>(json).Where(p => p.Position.FloorId == 109);

            List<PoiData> pois = new List<PoiData> ();
            List<PoiMetaData> poiMetaData = new List<PoiMetaData> ();
            int index = 0;
            foreach(var situmPoiData in situmPois)
            {
                index++;
                var (poi, poiMeta) = SitumPoiMapping.Map(situmPoiData);
                pois.Add (poi);
                poiMetaData.Add(poiMeta);
            }

            var poiJson = GetJson(pois);
            var poiMetaJson = GetJson(poiMetaData);

            File.WriteAllText(Path.Combine(outputPath, "Pois.json"), poiJson);
            File.WriteAllText(Path.Combine(outputPath, "PoiMetaData.json"), poiMetaJson);
        }

        private static string GetJson<T>(T value)
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }
    }
}
