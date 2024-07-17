using GeoJsonHelper.GeoJsonGeometries;
using GeoJsonHelper.GeoJsonObjects;
using Newtonsoft.Json.Linq;

namespace GeoJsonHelper.Factories
{
    internal sealed class GeoJsonObjectFactory : IGeoJsonObjectFactory
    {
        public GeoJson CreateGeoJson(JObject jObject, GeoJsonObjectTypes objectType)
        {
            GeoJson geoJson;
            switch (objectType)
            {
                case GeoJsonObjectTypes.Feature:
                    geoJson = new GeoJsonFeature();
                    break;
                case GeoJsonObjectTypes.FeatureCollection:
                    geoJson = new GeoJsonFeatureCollection();
                    break;
                case GeoJsonObjectTypes.GeometryCollection:
                    geoJson = new GeoJsonGeometryCollection();
                    break;
                case GeoJsonObjectTypes.LineString:
                    geoJson = new GeoJsonLineString();
                    break;
                case GeoJsonObjectTypes.MultiLineString:
                    geoJson = new GeoJsonMultiLineString();
                    break;
                case GeoJsonObjectTypes.MultiPoint:
                    geoJson = new GeoJsonMultiPoint();
                    break;
                case GeoJsonObjectTypes.MultiPolygon:
                    geoJson = new GeoJsonMultiPolygon();
                    break;
                case GeoJsonObjectTypes.Point:
                    geoJson = new GeoJsonPoint();
                    break;
                case GeoJsonObjectTypes.Polygon:
                    geoJson = new GeoJsonPolygon();
                    break;
                default:
                    geoJson = null;
                    break;
            }

            return geoJson;
        }
    }
}
