using System;

namespace WeatherCast.DataProvider
{
    internal class LastRequestCurrentInfo
    {
        public string CityName { get; set; } = Definitions.DefaultCity;

        public DateTime RequestTime { get; set; } = DateTime.MinValue;
    }
}
