using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyFootPrint : IMDFGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Guid[] Building_ids { get; set; }
    }
}
