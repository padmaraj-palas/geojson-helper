using System;
using System.Collections.Generic;
using System.IO;

//string filePath = Path.GetFullPath("./passenger_100124- FOR SPIKE.geojson");

//IGeoJsonService geoJsonService = new GeojsonService();
//var position = geoJsonService.Load(filePath);

//Console.WriteLine(position);

string csvFIlePath = "C:\\Users\\padmaraj.palas\\Downloads\\results.csv";
if (!File.Exists(csvFIlePath))
{
    Console.WriteLine($"File not found at {csvFIlePath}");
    return;
}

string[] keys = new string[0];
IList<string[]> values = new List<string[]>();

using (FileStream fileStream = File.OpenRead(csvFIlePath))
{
    using (TextReader textReader = new StreamReader(fileStream))
    {
        var line = textReader.ReadLine();
        if (line == null)
        {
            return;
        }

        keys = line.Split(',');

        line = textReader.ReadLine();
        values = new List<string[]>();
        while (line != null)
        {
            values.Add(line.Split(","));
            line = textReader.ReadLine();
        }
    }
}

string json = "[\n";

for(int v = 0; v < values.Count; v++)
{
    json += "{\n";
    for(int i = 0; i < keys.Length; i++)
    {
        json += $"\"{keys[i]}\"" + ":\t" + $"\"{values[v][i]}\"" + (i < keys.Length - 1 ? "," : string.Empty) + "\n";
    }

    json += "}" + (v < values.Count - 1 ? "," : string.Empty) + "\n";
}

json += "]\n";

Console.WriteLine(json);