using System;

namespace WeatherCast.DataProvider
{
    internal class LastRequestInfo
    {
        public LastRequestInfo(string cityName, string longitude, string latitude, DateTime requestTime)
        {
            CityName = cityName ?? throw new ArgumentNullException(nameof(cityName));
            Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
            Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
            RequestTime = requestTime;
        }

        public string CityName { get; }

        public string Longitude { get; }

        public string Latitude { get; }

        public DateTime RequestTime { get; }

        public string[] ToArray()
        {
            return new string[] { CityName, Longitude, Latitude, RequestTime.ToString() };
        }
    }
}
