using GeoJsonHelperConsole;
using System.IO;

string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./Floor 109_all.csv");
string svgMaskPath = Path.GetFullPath("./AUH_MAP_WITH_MASK TESTING.svg");
string poiOutputPath = "C:\\Users\\padmaraj.palas\\Desktop";
string jsonPoisPath = "./AUH POI master.json";

//await GeoJson2SvgCreator.Create(geoJsonPath, poisPath);

await PoiExporter.ExportFromJson(jsonPoisPath, poiOutputPath);

//var pois = await PoiExporter.GetPoisFromJson(jsonPoisPath);
//var excludedPois = await FilterPoiBasedOnGeoLocationMask.Filter(svgMaskPath, geoJsonPath, pois);

//File.WriteAllText(Path.Combine(poiOutputPath, "excluded_pois.json"), JsonContentSerializer.Serialize(excludedPois));
