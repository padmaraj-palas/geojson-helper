using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonObjects
{
    public sealed class GeoJsonFeatureCollection : GeoJson
    {
        [MaybeNull] public GeoJsonFeature[] Features { get; set; }
    }
}
