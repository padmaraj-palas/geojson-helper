using System;
using VectorMath;

namespace GeoPositioning
{
    public struct GeoPosition
    {
        public const uint RadiusOfEarthInMeters = 6371000;

        public static GeoPosition Origin => new GeoPosition(0, 0, 0);

        public GeoPosition(double latitude, double longitude)
            : this(latitude, longitude, 0)
        { }

        public GeoPosition(double latitude, double longitude, float altitude)
        {
            Altitude = altitude;
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public readonly override bool Equals(object obj)
        {
            return obj is GeoPosition position &&
                   Altitude == position.Altitude &&
                   Latitude == position.Latitude &&
                   Longitude == position.Longitude;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Altitude, Latitude, Longitude);
        }

        public readonly GeoPosition ToDeg()
        {
            return new GeoPosition { Altitude = Altitude, Latitude = Latitude * Constants.RadToDeg, Longitude = Longitude * Constants.RadToDeg };
        }

        public readonly GeoPosition ToRad()
        {
            return new GeoPosition { Altitude = Altitude, Latitude = Latitude * Constants.DegToRad, Longitude = Longitude * Constants.DegToRad };
        }

        public static bool operator ==(GeoPosition lhs, GeoPosition rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GeoPosition lhs, GeoPosition rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static GeoPosition operator +(GeoPosition lhs, GeoPosition rhs)
        {
            return new GeoPosition { Altitude = Math.Max(lhs.Altitude, rhs.Altitude), Latitude = lhs.Latitude + rhs.Latitude, Longitude = lhs.Longitude + rhs.Longitude };
        }

        public static GeoPosition operator -(GeoPosition lhs, GeoPosition rhs)
        {
            return new GeoPosition { Altitude = Math.Max(lhs.Altitude, rhs.Altitude), Latitude = lhs.Latitude - rhs.Latitude, Longitude = lhs.Longitude - rhs.Longitude };
        }

        public static GeoPosition operator *(GeoPosition lhs, double value)
        {
            return new GeoPosition { Altitude = lhs.Altitude, Latitude = lhs.Latitude * value, Longitude = lhs.Longitude * value };
        }

        public static GeoPosition operator *(double value, GeoPosition rhs)
        {
            return new GeoPosition { Altitude = rhs.Altitude, Latitude = rhs.Latitude * value, Longitude = rhs.Longitude * value };
        }

        public static GeoPosition operator /(GeoPosition lhs, double value)
        {
            return new GeoPosition { Altitude = lhs.Altitude, Latitude = lhs.Latitude / value, Longitude = lhs.Longitude / value };
        }



        public static double BearingInDegrees(GeoPosition p1, GeoPosition p2)
        {
            var p1Rad = p1.ToRad();
            var p2Rad = p2.ToRad();
            var diff = p2Rad - p1Rad;

            var y = Math.Sin(diff.Longitude) * Math.Cos(p2Rad.Latitude);
            var x = Math.Cos(p1Rad.Latitude) * Math.Sin(p2Rad.Latitude) -
                Math.Sin(p1Rad.Latitude) * Math.Cos(p2Rad.Latitude) * Math.Cos(diff.Longitude);
            var bearing = Math.Atan2(y, x);
            return (bearing * Constants.RadToDeg + 360) % 360;
        }

        public static double DistanceInMeters(GeoPosition p1, GeoPosition p2)
        {
            var p1Rad = p1.ToRad();
            var p2Rad = p2.ToRad();

            var diff = p2Rad - p1Rad;

            var a = Math.Sin(diff.Latitude / 2) * Math.Sin(diff.Latitude / 2) +
                Math.Cos(p1Rad.Latitude) * Math.Cos(p2Rad.Latitude) *
                Math.Sin(diff.Longitude / 2) * Math.Sin(diff.Longitude / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return RadiusOfEarthInMeters * c;
        }

        public static GeoPosition GetTargetPosition(GeoPosition start, double bearingInDegrees, double distance)
        {
            double ad = distance / RadiusOfEarthInMeters;
            var startRad = start.ToRad();
            var bearingInRad = bearingInDegrees * Constants.DegToRad;
            var lat = Math.Asin(Math.Sin(startRad.Latitude) * Math.Cos(ad) +
                Math.Cos(startRad.Latitude) * Math.Sin(ad) * Math.Cos(bearingInRad));

            var lon = startRad.Longitude + Math.Atan2(Math.Sin(bearingInRad) * Math.Sin(ad) * Math.Cos(startRad.Latitude),
                Math.Cos(ad) - Math.Sin(startRad.Latitude) * Math.Sin(lat));

            lon = ((lon + 540) % 360) - 180;
            return new GeoPosition(lat * Constants.RadToDeg, lon * Constants.RadToDeg);
        }

        public static Vector2 GetPositionInMeters(GeoPosition geoPosition, GeoPosition? origin)
        {
            var referencePoint = origin == null ? Origin : origin.Value;
            var distance = DistanceInMeters(referencePoint, geoPosition);
            var bearing = BearingInDegrees(referencePoint, geoPosition);
            var direction = RotatedNorth(Vector2.Up, -bearing);
            return direction * distance;
        }

        private static Vector2 RotatedNorth(Vector2 north, double bearingInDegrees)
        {
            var bearingInRad = bearingInDegrees * Constants.DegToRad;
            return new Vector2
            {
                X = north.X * Math.Cos(bearingInRad) - north.Y * Math.Sin(bearingInRad),
                Y = north.X * Math.Sin(bearingInRad) + north.Y * Math.Cos(bearingInRad)
            };
        }
    }
}
