using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CSVHelper;
using Newtonsoft.Json.Converters;

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

            _serializer.Converters.Add(new StringEnumConverter());
        }

        public static async Task Export(string geoJsonPath, string poisPath, string outputPath)
        {
            IDictionary<string, PoiData> gates = new Dictionary<string, PoiData>();
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
                if (poi.PoiType == PoiType.Gate)
                {
                    gates.Add(poi.Name, poi);
                }

                pois.Add(poi);
                poiMetaData.Add(poiMeta);
            }

            ProcessGates(gates);

            var poiJson = GetJson(pois);
            var poiMetaJson = GetJson(poiMetaData);

            File.WriteAllText(Path.Combine(outputPath, "Pois.json"), poiJson);
            File.WriteAllText(Path.Combine(outputPath, "PoiMetaData.json"), poiMetaJson);
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
