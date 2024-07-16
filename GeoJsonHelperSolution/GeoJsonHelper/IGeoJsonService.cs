using System;
using System.Collections.Generic;
using GeoJsonParser.Apple.GeoJsonFeatures;
using GeoJsonParser.Apple.Properties;
using GeoJsonParser.GeoJsonObjects;

namespace GeoJsonParser
{
    public interface IGeoJsonService
    {
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAddress>> Addresses { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAmenity>> Amenities { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyAnchor>> Anchors { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyBuilding>> Buildings { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyDetail>> Details { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFixture>> Fixtures { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyFootPrint>> Footprints { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyGeofence>> Geofences { get; }
        GeoJson GeoJson { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyKiosk>> Kiosks { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyLevel>> Levels { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOccupant>> Occupants { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyOpening>> Openings { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyRelationship>> Relationships { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertySection>> Sections { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyUnit>> Units { get; }
        IReadOnlyDictionary<Guid, AppleGeoJsonFeature<AppleGeoJsonPropertyVenue>> Venues { get; }

        internal void AddFeature(GeoJsonFeature feature);
        GeoJson Load(string filepath);
    }
}
