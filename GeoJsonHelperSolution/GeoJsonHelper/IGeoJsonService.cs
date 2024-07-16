using System;
using System.Collections.Generic;
using GeoJsonHelper.IMDF.GeoJsonFeatures;
using GeoJsonHelper.IMDF.Properties;
using GeoJsonHelper.GeoJsonObjects;

namespace GeoJsonHelper
{
    public interface IGeoJsonService
    {
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAddress>> Addresses { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAmenity>> Amenities { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyAnchor>> Anchors { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyBuilding>> Buildings { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyDetail>> Details { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFixture>> Fixtures { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyFootPrint>> Footprints { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyGeofence>> Geofences { get; }
        GeoJson GeoJson { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyKiosk>> Kiosks { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyLevel>> Levels { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOccupant>> Occupants { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyOpening>> Openings { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyRelationship>> Relationships { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertySection>> Sections { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyUnit>> Units { get; }
        IReadOnlyDictionary<Guid, IMDFGeoJsonFeature<IMDFGeoJsonPropertyVenue>> Venues { get; }

        internal void AddFeature(GeoJsonFeature feature);
        GeoJson Load(string filepath);
    }
}
