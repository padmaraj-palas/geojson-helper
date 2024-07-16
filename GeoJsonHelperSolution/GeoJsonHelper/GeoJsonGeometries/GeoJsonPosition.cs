namespace GeoJsonParser.GeoJsonGeometries
{
    public sealed class GeoJsonPosition
    {
        public decimal? Altitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public override string ToString()
        {
            if (Altitude == null)
            {
                return $"[{Longitude}, {Latitude}]";
            }

            return $"[{Longitude}, {Latitude}, {Altitude}]";
        }

        public static implicit operator GeoJsonPosition(decimal[] values)
        {
            if (values == null || values.Length == 0)
            {
                return null;
            }

            return new GeoJsonPosition
            {
                Altitude = values.Length > 2 ? values[2] : (decimal?)null,
                Longitude = values[0],
                Latitude = values[1]
            };
        }

        public static implicit operator decimal[](GeoJsonPosition position)
        {
            if (position == null)
            {
                return null;
            }

            if (position.Altitude == null)
            {
                return new decimal[] { position.Longitude, position.Latitude };
            }

            return new decimal[] { position.Longitude, position.Latitude, position.Altitude.Value };
        }
    }
}
