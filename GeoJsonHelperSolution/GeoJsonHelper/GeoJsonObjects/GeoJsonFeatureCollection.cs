using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.GeoJsonObjects
{
    public sealed class GeoJsonFeatureCollection : GeoJson
    {
        [MaybeNull] public GeoJsonFeature[] Features { get; set; }
    }
}
