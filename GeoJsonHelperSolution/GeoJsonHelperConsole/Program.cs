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
    var pois = PoiJsonSerializer.Deserialize<PoiData[]>(json).Where(p => p.Position.FloorId == 109);

    var building = geoJsonService.Buildings.Values.FirstOrDefault();
    var level = geoJsonService.Levels.Values.FirstOrDefault(l => l.Properties.Ordinal == 3);

    var displayPoint = building.Properties.Display_point.Coordinates;
    var origin = new GeoPosition((double)displayPoint.Latitude, (double)displayPoint.Longitude);

    var units = geoJsonService.Units.Values
        .Where(u => u.Properties.Level_id == level.Id)
        .Where(u => u.Properties.Category == "room" ||
            u.Properties.Category == "escalator" ||
            u.Properties.Category == "stairs" ||
            u.Properties.Category == "unspecified" ||
            u.Properties.Category == "lobby" ||
            u.Properties.Category == "elevator");

    var mapping = MapGeoJsonAndPoiMock(units, pois.ToList(), origin);
    GeoJson2SvgCreator.Create(geoJsonService.Venues.Values.FirstOrDefault().GetPositionsFromFeature(origin), mapping);
}

IList<(Vector2[] vertices, PoiData poi, string groupName)> MapGeoJsonAndPoiMock(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IEnumerable<PoiData> pois, GeoPosition origin)
{
    IList<(Vector2[], PoiData, string)> mapping = new List<(Vector2[], PoiData, string)>();

    int index = 0;
    foreach (var feature in features)
    {
        index++;
        var poiData = new PoiData
        {
            BuildingId = 29,
            Id = index,
            Name = $"Poi {index}",
            Type = "POI",
            Position = new PoiData.Pos
            {
                FloorId = 109,
                Latitude = (double)feature.Properties.Display_point.Coordinates.Latitude,
                Longitude = (double)feature.Properties.Display_point.Coordinates.Longitude,
                Georeferences = new PoiData.GeoRef
                {
                    Latitude = (double)feature.Properties.Display_point.Coordinates.Latitude,
                    Longitude = (double)feature.Properties.Display_point.Coordinates.Longitude
                }
            }
        };

        var vertices = feature.GetPositionsFromFeature(origin);
        mapping.Add((vertices, poiData, "PoiRoot"));
    }

    return mapping;
}

IList<(Vector2[] vertices, PoiData poi, string groupName)> MapGeoJsonAndPoiTries(IEnumerable<IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> features, IList<PoiData> pois, GeoPosition origin)
{
    int poiCount = pois.Count();
    IList<(Vector2[], PoiData, string)> mapping = new List<(Vector2[], PoiData, string)>();

    var poiId = 0;
    foreach (var feature in features)
    {
        var vertices = feature.GetPositionsFromFeature(origin);
        var clonedVertices = new Vector2[vertices.Length - 1];
        Array.Copy(vertices, clonedVertices, clonedVertices.Length);
        var triangles = EarClippingTriangulator.GetBaseTries(clonedVertices);

        var triList = new List<Vector2[]>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            var triangulatedVertices = new Vector2[3];
            for (int j = 0; j < 3; j++)
            {
                try
                {
                    triangulatedVertices[j] = clonedVertices[triangles[i + j]];
                }
                catch (Exception ex)
                {
                    int index = i + j;
                    Console.WriteLine(index);
                }
            }

            //triangulatedVertices[3] = clonedVertices[triangles[i]];
            triList.Add(triangulatedVertices);
            //mapping.Add((triangulatedVertices, poiData, string.IsNullOrEmpty(feature.Properties.Category) ? "unknown" : feature.Properties.Category));
        }

        foreach (var poiData in pois)
        {
            var poiGeoPosition = new GeoPosition(poiData.Position.Georeferences.Latitude, poiData.Position.Georeferences.Longitude);
            var poiPosition = GeoPosition.GetPositionInMeters(poiGeoPosition, origin);
            bool isInside = false;
            foreach (var tries in triList)
            {
                var polyBound = new PolyBound();
                polyBound.SetPoints(tries.ToList());
                if (polyBound.IsPointInside(poiPosition))
                {
                    isInside = true;
                    break;
                }
            }

            if (isInside)
            {
                mapping.Add((vertices, poiData, "PoiRoot"));
                pois.Remove(poiData);
                break;
            }
        }
    }

    return mapping;
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
