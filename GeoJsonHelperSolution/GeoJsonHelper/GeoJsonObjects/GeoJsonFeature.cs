using System;
using System.Diagnostics.CodeAnalysis;
using GeoJsonParser.GeoJsonGeometries;

namespace GeoJsonParser.GeoJsonObjects
{
    public class GeoJsonFeature : GeoJson
    {
        [MaybeNull] public GeoJsonGeometry Geometry { get; set; }
        [MaybeNull] public Guid? Id { get; set; }
    }
}
