using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using System.Collections.Generic;
using System;
using GeoJsonHelper.GeoJsonObjects;
using GeoJsonHelper.IMDF;
using GeoJsonHelper.CustomConverters;

namespace GeoJsonHelper
{
    public sealed class IMDFGeoJsonService : GeojsonService, IIMDFGeoJsonService
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

        public IMDFGeoJsonService()
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
        }

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>> Addresses => _addresses;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>> Amenities => _amenities;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>> Anchors => _anchors;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>> Buildings => _buildings;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>> Details => _details;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>> Fixtures => _fixtures;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>> Footprints => _footprints;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>> Geofences => _geofences;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>> Kiosks => _kiosks;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>> Levels => _levels;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>> Occupants => _occupants;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>> Openings => _openings;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>> Relationships => _relationships;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>> Sections => _sections;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> Units => _units;

        public IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>> Venues => _venues;

        protected override IGeoJsonCreatedCallback GetGeoJsonCreatedCallback()
        {
            return new GeoJsonCreatedCallback(() => this);
        }

        private sealed class GeoJsonCreatedCallback : IGeoJsonCreatedCallback
        {
            private readonly Func<IMDFGeoJsonService> _geoJsonServiceGetter;

            public GeoJsonCreatedCallback(Func<IMDFGeoJsonService> geoJsonServiceGetter)
            {
                _geoJsonServiceGetter = geoJsonServiceGetter;
            }

            public void OnGeoJsonCreated(GeoJson geoJson)
            {
                if (geoJson is IMDFGeoJsonFeature feature)
                {
                    if (feature == null || feature.Id == null)
                    {
                        return;
                    }

                    var geoJsonService = _geoJsonServiceGetter?.Invoke();
                    var id = feature.Id.Value;
                    switch (feature.Feature_type)
                    {
                        case FeatureTypes.address:
                            geoJsonService._addresses[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>;
                            break;
                        case FeatureTypes.amenity:
                            geoJsonService._amenities[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>;
                            break;
                        case FeatureTypes.anchor:
                            geoJsonService._anchors[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>;
                            break;
                        case FeatureTypes.building:
                            geoJsonService._buildings[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>;
                            break;
                        case FeatureTypes.detail:
                            geoJsonService._details[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>;
                            break;
                        case FeatureTypes.fixture:
                            geoJsonService._fixtures[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>;
                            break;
                        case FeatureTypes.footprint:
                            geoJsonService._footprints[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>;
                            break;
                        case FeatureTypes.geofence:
                            geoJsonService._geofences[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>;
                            break;
                        case FeatureTypes.kiosk:
                            geoJsonService._kiosks[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>;
                            break;
                        case FeatureTypes.level:
                            geoJsonService._levels[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>;
                            break;
                        case FeatureTypes.occupant:
                            geoJsonService._occupants[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>;
                            break;
                        case FeatureTypes.opening:
                            geoJsonService._openings[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>;
                            break;
                        case FeatureTypes.relationship:
                            geoJsonService._relationships[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>;
                            break;
                        case FeatureTypes.section:
                            geoJsonService._sections[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>;
                            break;
                        case FeatureTypes.unit:
                            geoJsonService._units[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>;
                            break;
                        case FeatureTypes.venue:
                            geoJsonService._venues[id] = feature as IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>;
                            break;
                    }
                }
            }
        }
    }
}
