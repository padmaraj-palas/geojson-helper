using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyOccupant : IMDFGeoJsonProperty
    {
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public Guid? Anchor_id { get; set; }
        [MaybeNull] public string Hours { get; set; }
        [MaybeNull] public string Phone { get; set; }
        [MaybeNull] public string Website { get; set; }
        [MaybeNull] public Temporality Validity { get; set; }
        [MaybeNull] public Guid? Correlation_id { get; set; }
    }
}
