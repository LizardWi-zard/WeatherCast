using System;

namespace WeatherCast.DataProvider
{
    internal class LastRequestInfo
    {
        public string CityName { get; set; } = Definitions.DefaultCity;

        public string Longitude { get; set; } = Definitions.DefaultLongitude;

        public string Latitude { get; set; } = Definitions.DefaultLatitude;

        public DateTime RequestTime { get; set; } = DateTime.MinValue;
    }
}
