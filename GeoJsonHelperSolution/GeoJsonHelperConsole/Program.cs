using GeoJsonHelperConsole;
using System.IO;

string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./results.csv");
string poiOutputPath = "C:\\Users\\padmaraj.palas\\Desktop";

await GeoJson2SvgCreator.Create(geoJsonPath, poisPath);

//await MockPoiExporter.Export(geoJsonPath, poisPath, poiOutputPath);
