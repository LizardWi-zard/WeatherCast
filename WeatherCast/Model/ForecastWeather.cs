using System.Collections.Generic;

namespace WeatherCast.Model
{
    public class ForecastWeather
    {
        public float Lat { get; set; }

        public float Lon { get; set; }

        public string TimeZone { get; set; }

        public DailyCast[] Daily { get; set; }

        public HourCast[] Hourly { get; set; }

        public IEnumerable<HourCast> ForecastFor24Hours { get; set; }
    }
}
