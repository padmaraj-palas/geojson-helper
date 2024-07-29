using GeoJsonHelperConsole;
using System.IO;

string geoJsonPath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");
string poisPath = Path.GetFullPath("./results.csv");

//await GeoJson2SvgCreator.Create(geoJsonPath, poisPath);

await MockPoiExporter.Export(geoJsonPath, poisPath, "");
