using System;
using System.Collections.Generic;
using GeoJsonParser.Apple;
using GeoJsonParser.Apple.GeoJsonFeatures;
using GeoJsonParser.Apple.Properties;
using GeoJsonParser.GeoJsonObjects;

namespace GeoJsonParser
{
    public sealed class GeojsonService : IGeoJsonService
    {
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>> _addresses;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>> _amenities;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>> _anchors;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>> _buildings;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>> _details;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>> _fixtures;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>> _footprints;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>> _geofences;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>> _kiosks;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>> _levels;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>> _occupants;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>> _openings;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>> _relationships;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertySection>> _sections;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>> _units;
        private readonly Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>> _venues;

        private GeoJson _geoJson;

        public GeojsonService()
        {
            _addresses = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>>();
            _amenities = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>>();
            _anchors = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>>();
            _buildings = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>>();
            _details = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>>();
            _fixtures = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>>();
            _footprints = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>>();
            _geofences = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>>();
            _kiosks = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>>();
            _levels = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>>();
            _occupants = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>>();
            _openings = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>>();
            _relationships = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>>();
            _sections = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertySection>>();
            _units = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>>();
            _venues = new Dictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>>();

            GeoJsonParser.Init(this);
        }

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>> Addresses => _addresses;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>> Amenities => _amenities;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>> Anchors => _anchors;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>> Buildings => _buildings;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>> Details => _details;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>> Fixtures => _fixtures;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>> Footprints => _footprints;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>> Geofences => _geofences;
        
        public GeoJson GeoJson => _geoJson;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>> Kiosks => _kiosks;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>> Levels => _levels;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>> Occupants => _occupants;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>> Openings => _openings;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>> Relationships => _relationships;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertySection>> Sections => _sections;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>> Units => _units;

        public IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>> Venues => _venues;

        void IGeoJsonService.AddFeature(GeoJsonFeature feature)
        {
            if (feature == null || !(feature is AppleGeoJsonFeature) || feature.Id == null)
            {
                return;
            }

            var appleGeoJsonFeature = feature as AppleGeoJsonFeature;
            var id = feature.Id.Value;

            switch (appleGeoJsonFeature.Feature_type)
            {
                case FeatureTypes.address:
                    _addresses[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>;
                    break;
                case FeatureTypes.amenity:
                    _amenities[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>;
                    break;
                case FeatureTypes.anchor:
                    _anchors[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>;
                    break;
                case FeatureTypes.building:
                    _buildings[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>;
                    break;
                case FeatureTypes.detail:
                    _details[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>;
                    break;
                case FeatureTypes.fixture:
                    _fixtures[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>;
                    break;
                case FeatureTypes.footprint:
                    _footprints[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>;
                    break;
                case FeatureTypes.geofence:
                    _geofences[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>;
                    break;
                case FeatureTypes.kiosk:
                    _kiosks[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>;
                    break;
                case FeatureTypes.level:
                    _levels[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>;
                    break;
                case FeatureTypes.occupant:
                    _occupants[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>;
                    break;
                case FeatureTypes.opening:
                    _openings[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>;
                    break;
                case FeatureTypes.relationship:
                    _relationships[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>;
                    break;
                case FeatureTypes.section:
                    _sections[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertySection>;
                    break;
                case FeatureTypes.unit:
                    _units[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>;
                    break;
                case FeatureTypes.venue:
                    _venues[id] = appleGeoJsonFeature as AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>;
                    break;
            }
        }

        public GeoJson Load(string filepath)
        {
            return _geoJson = GeoJsonParser.Serialize<GeoJson>(filepath);
        }
    }
}
