using GeoJsonHelper.GeoJsonGeometries;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoJsonHelper.IMDF.Properties
{
    public sealed class IMDFGeoJsonPropertyLevel : IMDFGeoJsonProperty
    {
        [MaybeNull] public string Category { get; set; }
        [MaybeNull] public string Restriction { get; set; }
        public bool Outdoor { get; set; }
        public int Ordinal { get; set; }
        [MaybeNull] public Label Name { get; set; }
        [MaybeNull] public Label Short_name { get; set; }
        [MaybeNull] public GeoJsonPoint Display_point { get; set; }
        [MaybeNull] public Guid? Address_id { get; set; }
        [MaybeNull] public Guid[] Building_ids { get; set; }
    }
}
