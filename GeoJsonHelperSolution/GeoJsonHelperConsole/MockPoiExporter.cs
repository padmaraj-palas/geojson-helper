using GeoPositioning;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CSVHelper;

namespace GeoJsonHelperConsole
{
    public static class MockPoiExporter
    {
        private static readonly Random _random;
        private static readonly JsonSerializer _serializer;

        static MockPoiExporter()
        {
            _random = new Random(100);
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

            string type = "Venue";
            List<PoiData> pois = new List<PoiData> ();
            List<PoiMetaData> poiMetaData = new List<PoiMetaData> ();
            int index = 0;
            foreach(var situmPoi in situmPois)
            {
                index++;
                var poi = new PoiData
                {
                    Id = index,
                    Categories = new List<string>(),
                    Description = situmPoi.Name,
                    Name = situmPoi.Name,
                    NameInvariant = situmPoi.Name,
                    NavigationId = Guid.NewGuid().ToString(),
                    NavigationType = "Poi",
                    PoiType = type,
                    SlotId = index,
                    ControlledByPois = new List<int>(),
                    LogoImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/A013748470C459A8AF1F58316EAC17F7.png",
                    MenuImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/A013748470C459A8AF1F58316EAC17F7.png",
                    DisplayAttribute = "label"//_random.Next(1, 10) % 2 == 0 ? "Icon" : "Label"
                };
                var poiMeta = new PoiMetaData
                {
                    Position = new GeoPosition(situmPoi.Position.Georeferences.Latitude, situmPoi.Position.Georeferences.Longitude),
                    Priority = GetPriorityFromSitumPoi(situmPoi),
                    SlotId = index
                };
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

        private static int GetPriorityFromSitumPoi(SitumPoiData situmPoiData)
        {
            try
            {
                if (situmPoiData.CustomFields != null)
                {
                    var customField = situmPoiData.CustomFields.FirstOrDefault(cf => cf.Key == "priority");
                    if (customField != null)
                    {
                        return int.Parse(customField.Value);
                    }
                }
            }
            catch
            { }

            return 0;
        }
    }
}
