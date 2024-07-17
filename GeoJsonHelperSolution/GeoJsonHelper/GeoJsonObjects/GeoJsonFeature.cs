using System;

using GeoJsonHelper.GeoJsonGeometries;

namespace GeoJsonHelper.GeoJsonObjects
{
    public class GeoJsonFeature : GeoJson
    {
        public GeoJsonGeometry Geometry { get; set; }
        public Guid? Id { get; set; }
    }
}
