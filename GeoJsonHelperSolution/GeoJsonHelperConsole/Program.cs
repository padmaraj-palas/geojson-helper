using CSVHelper;
using GeoJsonHelper;
using GeoJsonHelper.GeoJsonObjects;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoJsonHelperConsole;
using GeoPositioning;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VectorMath;

var random = new Random(100);
string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./results.csv");

IGeoJsonService geoJsonService = new GeojsonService();
await geoJsonService.LoadAsync(geoJsonPath, OnGeoJsonLoadCompleted);
await CSVParser.ParseAsync(poisPath, OnPoiRecordLoadCompleted);

void OnGeoJsonLoadCompleted(GeoJson json)
{
    if (json == null)
    {
        Console.WriteLine($"GeoJson is empty at {geoJsonPath}");
        return;
    }

    Console.WriteLine(json);
}

void OnPoiRecordLoadCompleted(CsvRecord[] records)
{
    if (records == null || records.Length == 0)
    {
        Console.WriteLine($"Poi record is empty at {poisPath}");
        return;
    }

    var json = records.ToJson();
    JArray jArray = JArray.Parse(json);
    var pois = PoiJsonSerializer.Deserialize<PoiData[]>(json);

    var building = geoJsonService.Buildings.Values.FirstOrDefault();
    var level = geoJsonService.Levels.Values.FirstOrDefault(l => l.Properties.Ordinal == 3);

    var displayPoint = building.Properties.Display_point.Coordinates;
    var origin = new GeoPosition((double)displayPoint.Latitude, (double)displayPoint.Longitude);

    var units = geoJsonService.Units.Values
        .Where(u => u.Properties.Level_id == level.Id);

    var mapping = MapGeoJsonAndPoi(units, pois, origin);
    GeoJson2SvgCreator.Create(level, origin, mapping);
}

IList<(GeoJsonFeature feature, PoiData poi, string groupName)> MapGeoJsonAndPoi(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<PoiData> pois, GeoPosition origin)
{
    IList<(GeoJsonFeature, PoiData, string)> mapping = new List<(GeoJsonFeature, PoiData, string)>();

    foreach (var feature in features)
    {
        foreach (PoiData poiData in pois)
        {
            var poiGeoPosition = new GeoPosition(poiData.Position.Latitude, poiData.Position.Longitude);
            var poiPosition = GeoPosition.GetPositionInMeters(poiGeoPosition, origin);
            var vertices = feature.GetPositionsFromFeature(origin);
            var verticeList = vertices.ToList();
            verticeList.RemoveAt(verticeList.Count - 1);
            var polyBound = new PolyBound();
            polyBound.SetPoints(verticeList);
            if (polyBound.IsPointInside(poiPosition))
            {
                mapping.Add((feature, poiData, string.IsNullOrEmpty(feature.Properties.Category) ? "unknown" : feature.Properties.Category));
                break;
            }
        }
    }

    return mapping;
}
