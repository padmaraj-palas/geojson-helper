using GeoJsonHelperConsole;
using System.IO;

string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./Floor 109_all.csv");
string svgMaskPath = Path.GetFullPath("./AUH_MAP_WITH_MASK.svg");
string poiOutputPath = "C:\\Users\\padmaraj.palas\\Desktop";

//await GeoJson2SvgCreator.Create(geoJsonPath, poisPath);

//await PoiExporter.Export(geoJsonPath, poisPath, poiOutputPath);

var pois = await PoiExporter.GetPois(poisPath);
var excludedPois = await FilterPoiBasedOnGeoLocationMask.Filter(svgMaskPath, geoJsonPath, pois);

File.WriteAllText(Path.Combine(poiOutputPath, "excluded_pois.json"), JsonContentSerializer.Serialize(excludedPois));
