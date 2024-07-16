using System;
using System.Diagnostics.CodeAnalysis;
using GeoJsonHelper.GeoJsonGeometries;

namespace GeoJsonHelper.GeoJsonObjects
{
    public class GeoJsonFeature : GeoJson
    {
        [MaybeNull] public GeoJsonGeometry Geometry { get; set; }
        [MaybeNull] public Guid? Id { get; set; }
    }
}
