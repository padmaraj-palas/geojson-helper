using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class Door
    {
        [MaybeNull] public string Type { get; set; }
        public bool Automatic {  get; set; }
        [MaybeNull] public string Material { get; set; }
    }
}
