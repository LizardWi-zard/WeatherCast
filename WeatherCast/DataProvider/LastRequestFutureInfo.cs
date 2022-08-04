using System;

namespace WeatherCast.DataProvider
{
    internal class LastRequestForecastInfo
    {
        public string Longitude { get; set; } = Definitions.DefaultLongitude;

        public string Latitude { get; set; } = Definitions.DefaultLatitude;

        public DateTime RequestTime { get; set; } = DateTime.MinValue;
    }
}
