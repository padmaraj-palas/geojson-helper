using GeoJsonHelper;
using GeoPositioning;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

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
            IIMDFGeoJsonService geoJsonService = new IMDFGeoJsonService();
            var geoJson = await geoJsonService.LoadAsync(geoJsonPath);

            var building = geoJsonService.Buildings.Values.FirstOrDefault();

            var displayPoint = building.Properties.Display_point.Coordinates;
            var origin = new GeoPosition((double)displayPoint.Latitude, (double)displayPoint.Longitude);

            string type = "Venue";
            List<PoiData> pois = new List<PoiData> ();
            List<PoiLocationData> poiLocations = new List<PoiLocationData> ();
            for (int i = 0; i < 300; i++)
            {
                var poi = new PoiData
                {
                    Id = i + 1,
                    Categories = new List<string>(),
                    Description = $"Test Poi {i + 1}",
                    Name = $"Poi {i + 1}",
                    NameInvariant = $"Poi {i + 1}",
                    NavigationId = Guid.NewGuid().ToString(),
                    NavigationType = "Poi",
                    PoiType = type,
                    SlotId = i + 1,
                    ControlledByPois = new List<int>(),
                    LogoImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/A013748470C459A8AF1F58316EAC17F7.png",
                    MenuImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/A013748470C459A8AF1F58316EAC17F7.png",
                    DisplayAttribute = _random.Next(1, 10) % 2 == 0 ? "Icon" : "Label"
                };
                var poiLocation = new PoiLocationData
                {
                    Position = GeoPosition.GetTargetPosition(origin, _random.NextDouble() * 360, _random.Next(100, 500)),
                    SlotId = i + 1
                };
                pois.Add (poi);
                poiLocations.Add(poiLocation);
            }
            var poiJson = GetJson(pois);
            var poiLocationJson = GetJson(poiLocations);
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
