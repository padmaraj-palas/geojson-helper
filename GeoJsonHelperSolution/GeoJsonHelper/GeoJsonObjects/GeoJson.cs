using System.Diagnostics.CodeAnalysis;

namespace GeoJsonParser.GeoJsonObjects
{
    public abstract class GeoJson
    {
        [MaybeNull] public decimal[] Bbox { get; set; }
        public GeoJsonObjectTypes Type { get; set; }
    }
}
