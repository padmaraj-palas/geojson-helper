using GeoJsonHelper;
using System;
using System.IO;

string filePath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");

IGeoJsonService geoJsonService = new GeojsonService();
var position = geoJsonService.Load(filePath);

Console.WriteLine(position);
