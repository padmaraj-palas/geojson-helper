using System;
using GeoJsonParser.Apple.GeoJsonFeatures;
using GeoJsonParser.Apple.Properties;
using GeoJsonParser.Factories;
using GeoJsonParser.GeoJsonObjects;
using Newtonsoft.Json.Linq;

namespace GeoJsonParser.Apple.Factories
{
    internal sealed class AppleGeoJsonObjectFactory : IGeoJsonObjectFactory
    {
        private readonly IGeoJsonObjectFactory _geoJsonObjectFactory;

        public AppleGeoJsonObjectFactory(IGeoJsonObjectFactory geoJsonObjectFactory)
        {
            _geoJsonObjectFactory = geoJsonObjectFactory;
        }

        public GeoJson? CreateGeoJson(JObject jObject, GeoJsonObjectTypes objectType)
        {
            switch (objectType)
            {
                case GeoJsonObjectTypes.Feature:
                    return CreateGeoJsonWithProperty(jObject);
                default:
                    return _geoJsonObjectFactory.CreateGeoJson(jObject, objectType);
            }
        }

        public GeoJson? CreateGeoJsonWithProperty(JObject jObject)
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
                return new AppleGeoJsonFeature();
            }

            AppleGeoJsonFeature feature;
            switch (featureType)
            {
                case FeatureTypes.address:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>();
                    break;
                case FeatureTypes.amenity:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>();
                    break;
                case FeatureTypes.anchor:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>();
                    break;
                case FeatureTypes.building:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>();
                    break;
                case FeatureTypes.detail:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>();
                    break;
                case FeatureTypes.fixture:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>();
                    break;
                case FeatureTypes.footprint:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>();
                    break;
                case FeatureTypes.geofence:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>();
                    break;
                case FeatureTypes.kiosk:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>();
                    break;
                case FeatureTypes.level:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>();
                    break;
                case FeatureTypes.occupant:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>();
                    break;
                case FeatureTypes.opening:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>();
                    break;
                case FeatureTypes.relationship:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>();
                    break;
                case FeatureTypes.section:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertySection>();
                    break;
                case FeatureTypes.unit:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>();
                    break;
                case FeatureTypes.venue:
                    feature = new AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>();
                    break;
                default:
                    feature = new AppleGeoJsonFeature();
                    break;
            }

            return feature;
        }
    }
}
