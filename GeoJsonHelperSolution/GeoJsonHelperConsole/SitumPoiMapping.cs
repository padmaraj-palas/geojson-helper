using GeoPositioning;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GeoJsonHelperConsole
{
    public static class SitumPoiMapping
    {
        private const string PoiType = "Venue";

        private static readonly Random _random;

        static SitumPoiMapping()
        {
            _random = new Random(100);
        }

        public static (PoiData poiData, PoiMetaData poiMetaData) Map(SitumPoiData situmPoiData)
        {
            var poi = new PoiData
            {
                Id = situmPoiData.Id,
                Categories = new List<string>(),
                Description = situmPoiData.Name,
                Name = situmPoiData.Name,
                NameInvariant = situmPoiData.Name,
                NavigationId = Guid.NewGuid().ToString(),
                NavigationType = "Poi",
                PoiType = situmPoiData.CategoryName == "Boarding Gates" ? GeoJsonHelperConsole.PoiType.Gate : GeoJsonHelperConsole.PoiType.Venue,
                SlotId = situmPoiData.Id,
                ControlledByPois = new List<int>(),
                LogoImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/Resources/poi_icon_temp.png",
                MenuImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/Resources/poi_icon_temp.png",
                DisplayAttribute = "label"//_random.Next(1, 10) % 2 == 0 ? "Icon" : "Label"
            };
            var poiMeta = new PoiMetaData
            {
                Position = new GeoPosition(situmPoiData.Position.Georeferences.Latitude, situmPoiData.Position.Georeferences.Longitude),
                Priority = GetPriorityFromSitumPoi(situmPoiData),
                SlotId = situmPoiData.Id
            };

            return (poi, poiMeta);
        }

        public static (PoiData poiData, PoiMetaData poiMetaData) Map(FlatenedSitumPoiData situmPoiData)
        {
            var poi = new PoiData
            {
                Id = situmPoiData.Id,
                Categories = new List<string>(),
                Description = situmPoiData.Name,
                Name = situmPoiData.Name,
                NameInvariant = situmPoiData.Name,
                NavigationId = Guid.NewGuid().ToString(),
                NavigationType = "Poi",
                PoiType = GeoJsonHelperConsole.PoiType.Venue,
                SlotId = situmPoiData.Id,
                ControlledByPois = new List<int>(),
                LogoImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/Resources/poi_icon_temp.png",
                MenuImage = "https://ipendevstorage.blob.core.windows.net/ipen-abudhabi/Resources/poi_icon_temp.png",
                DisplayAttribute = "label"//_random.Next(1, 10) % 2 == 0 ? "Icon" : "Label"
            };
            var poiMeta = new PoiMetaData
            {
                Position = new GeoPosition(situmPoiData.Latitude, situmPoiData.Longitude),
                Priority = situmPoiData.Priority,
                SlotId = situmPoiData.Id
            };

            return (poi, poiMeta);
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
