using System;

namespace WeatherCast.DataProvider
{
    internal class LastRequestInfo
    {
        public LastRequestInfo(string cityName, DateTime requestTime)
        {
            CityName = cityName ?? throw new ArgumentNullException(nameof(cityName));
            RequestTime = requestTime;
        }

        public string CityName { get; }

        public DateTime RequestTime { get; }

        public string[] ToArray()
        {
            return new string[] { CityName, RequestTime.ToString() };
        }
    }
}
