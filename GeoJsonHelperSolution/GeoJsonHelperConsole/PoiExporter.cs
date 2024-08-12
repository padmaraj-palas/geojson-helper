using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CSVHelper;

namespace GeoJsonHelperConsole
{
    public static class PoiExporter
    {
        public static async Task Export(string geoJsonPath, string poisPath, string outputPath)
        {
            var (pois, poiMetaData) = await GetPois(poisPath);

            var poiJson = JsonContentSerializer.Serialize(pois);
            var poiMetaJson = JsonContentSerializer.Serialize(poiMetaData);

            File.WriteAllText(Path.Combine(outputPath, "Pois.json"), poiJson);
            File.WriteAllText(Path.Combine(outputPath, "PoiMetaData.json"), poiMetaJson);
        }

        public static async Task<(List<PoiData> pois, List<PoiMetaData> poiMetaData)> GetPois(string poisPath)
        {
            IDictionary<string, PoiData> gates = new Dictionary<string, PoiData>();
            var records = await CSVParser.ParseAsync(poisPath);
            var json = records.ToJson();
            var situmPois = PoiJsonSerializer.Deserialize<SitumPoiData[]>(json).Where(p => p.Position.FloorId == 109);

            List<PoiData> pois = new List<PoiData>();
            List<PoiMetaData> poiMetaData = new List<PoiMetaData>();
            int index = 0;
            foreach (var situmPoiData in situmPois)
            {
                index++;
                var (poi, poiMeta) = SitumPoiMapping.Map(situmPoiData);
                if (poi.PoiType == PoiType.Gate)
                {
                    gates.Add(poi.Name, poi);
                }

                pois.Add(poi);
                poiMetaData.Add(poiMeta);
            }

            ProcessGates(gates);

            return (pois, poiMetaData);
        }

        private static void ProcessGates(IDictionary<string, PoiData> gates)
        {
            foreach (var gate in gates.Values)
            {
                try
                {
                    int.Parse($"{gate.Name[gate.Name.Length - 1]}");
                }
                catch
                {
                    var gateName = gate.Name.Remove(gate.Name.Length - 1);
                    if (gates.TryGetValue(gateName, out PoiData parentGate))
                    {
                        parentGate.PoiType = PoiType.Group;
                        gate.PoiGroupId = parentGate.Id;
                    }
                }
            }
        }
    }
}
