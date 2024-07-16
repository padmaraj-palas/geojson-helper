using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyAnchor : IMDFGeoJsonProperty
    {
        [MaybeNull] public Guid? Address_id { get; set; }
        [MaybeNull] public Guid? Unit_id { get; set; }
    }
}
