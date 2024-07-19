using System;
using System.Collections.Generic;
using GeoJsonHelper.IMDF;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoJsonHelper.GeoJsonObjects;
using System.Threading.Tasks;

namespace GeoJsonHelper
{
    public sealed class GeojsonService : IGeoJsonService
    {
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>> _addresses;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>> _amenities;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>> _anchors;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>> _buildings;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>> _details;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>> _fixtures;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>> _footprints;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>> _geofences;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>> _kiosks;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>> _levels;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>> _occupants;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>> _openings;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>> _relationships;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>> _sections;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> _units;
        private readonly Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>> _venues;

        private GeoJson _geoJson;

        public GeojsonService()
        {
            _addresses = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>>();
            _amenities = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>>();
            _anchors = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>>();
            _buildings = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>>();
            _details = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>>();
            _fixtures = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>>();
            _footprints = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>>();
            _geofences = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>>();
            _kiosks = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>>();
            _levels = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>>();
            _occupants = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>>();
            _openings = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>>();
            _relationships = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>>();
            _sections = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>>();
            _units = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>>();
            _venues = new Dictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>>();

            GeoJsonParser.Init(this);
        }

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>> Addresses => _addresses;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>> Amenities => _amenities;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>> Anchors => _anchors;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>> Buildings => _buildings;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>> Details => _details;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>> Fixtures => _fixtures;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>> Footprints => _footprints;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>> Geofences => _geofences;
        
        public GeoJson GeoJson => _geoJson;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>> Kiosks => _kiosks;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>> Levels => _levels;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>> Occupants => _occupants;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>> Openings => _openings;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>> Relationships => _relationships;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>> Sections => _sections;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> Units => _units;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>> Venues => _venues;

        void IGeoJsonService.AddFeature(GeoJsonFeature feature)
        {
            if (feature == null || !(feature is IMDFGeoJsonFeature) || feature.Id == null)
            {
                return;
            }

            var appleGeoJsonFeature = feature as IMDFGeoJsonFeature;
            var id = feature.Id.Value;

            switch (appleGeoJsonFeature.Feature_type)
            {
                case FeatureTypes.address:
                    _addresses[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>;
                    break;
                case FeatureTypes.amenity:
                    _amenities[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>;
                    break;
                case FeatureTypes.anchor:
                    _anchors[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>;
                    break;
                case FeatureTypes.building:
                    _buildings[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>;
                    break;
                case FeatureTypes.detail:
                    _details[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>;
                    break;
                case FeatureTypes.fixture:
                    _fixtures[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>;
                    break;
                case FeatureTypes.footprint:
                    _footprints[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>;
                    break;
                case FeatureTypes.geofence:
                    _geofences[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>;
                    break;
                case FeatureTypes.kiosk:
                    _kiosks[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>;
                    break;
                case FeatureTypes.level:
                    _levels[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>;
                    break;
                case FeatureTypes.occupant:
                    _occupants[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>;
                    break;
                case FeatureTypes.opening:
                    _openings[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>;
                    break;
                case FeatureTypes.relationship:
                    _relationships[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>;
                    break;
                case FeatureTypes.section:
                    _sections[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>;
                    break;
                case FeatureTypes.unit:
                    _units[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>;
                    break;
                case FeatureTypes.venue:
                    _venues[id] = appleGeoJsonFeature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>;
                    break;
            }
        }

        public GeoJson Load(string filepath)
        {
            return _geoJson = GeoJsonParser.Serialize<GeoJson>(filepath);
        }

        public Task LoadAsync(string filepath, Action<GeoJson> onComplete)
        {
            return Task.Run(() =>
            {
                var geoJson = Load(filepath);
                onComplete(geoJson);
            });
        }
    }
}
