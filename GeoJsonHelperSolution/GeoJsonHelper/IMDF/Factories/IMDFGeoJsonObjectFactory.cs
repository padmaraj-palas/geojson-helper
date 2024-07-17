using System;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoJsonHelper.Factories;
using GeoJsonHelper.GeoJsonObjects;
using Newtonsoft.Json.Linq;

namespace GeoJsonHelper.IMDF.Factories
{
    internal sealed class IMDFGeoJsonObjectFactory : IGeoJsonObjectFactory
    {
        private readonly IGeoJsonObjectFactory _geoJsonObjectFactory;

        public IMDFGeoJsonObjectFactory(IGeoJsonObjectFactory geoJsonObjectFactory)
        {
            _geoJsonObjectFactory = geoJsonObjectFactory;
        }

        public GeoJson CreateGeoJson(JObject jObject, GeoJsonObjectTypes objectType)
        {
            switch (objectType)
            {
                case GeoJsonObjectTypes.Feature:
                    return CreateGeoJsonWithProperty(jObject);
                default:
                    return _geoJsonObjectFactory.CreateGeoJson(jObject, objectType);
            }
        }

        public GeoJson CreateGeoJsonWithProperty(JObject jObject)
        {
            FeatureTypes featureType;
            try
            {
                JToken featureTypeToken;
                if (!jObject.TryGetValue("feature_type", out featureTypeToken))
                {
                    throw new ArgumentException("AppleGeoJsonObjectFactory: Feature Type not specified");
                }

                featureType = Enum.Parse<FeatureTypes>(featureTypeToken.ToString(), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new IMDFGeoJsonFeature();
            }

            IMDFGeoJsonFeature feature;
            switch (featureType)
            {
                case FeatureTypes.address:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>();
                    break;
                case FeatureTypes.amenity:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>();
                    break;
                case FeatureTypes.anchor:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>();
                    break;
                case FeatureTypes.building:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>();
                    break;
                case FeatureTypes.detail:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>();
                    break;
                case FeatureTypes.fixture:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>();
                    break;
                case FeatureTypes.footprint:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>();
                    break;
                case FeatureTypes.geofence:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>();
                    break;
                case FeatureTypes.kiosk:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>();
                    break;
                case FeatureTypes.level:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>();
                    break;
                case FeatureTypes.occupant:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>();
                    break;
                case FeatureTypes.opening:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>();
                    break;
                case FeatureTypes.relationship:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>();
                    break;
                case FeatureTypes.section:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>();
                    break;
                case FeatureTypes.unit:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>();
                    break;
                case FeatureTypes.venue:
                    feature = new IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>();
                    break;
                default:
                    feature = new IMDFGeoJsonFeature();
                    break;
            }

            return feature;
        }
    }
}
