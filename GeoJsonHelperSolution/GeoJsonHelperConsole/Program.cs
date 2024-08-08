using GeoJsonHelperConsole;
using System.IO;

string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./Floor 109_all.csv");
string poiOutputPath = "C:\\Users\\padmaraj.palas\\Desktop";

//await GeoJson2SvgCreator.Create(geoJsonPath, poisPath);

await PoiExporter.Export(geoJsonPath, poisPath, poiOutputPath);
