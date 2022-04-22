using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast
{
    public class ForecastWeather
    {
        public float Lat { get; set; }

        public float Lon { get; set; }

        public string TimeZone { get; set; }

        public DailyCast[] Daily { get; set; }
    }

    public class DailyCast
    {
        public int Dt { get; set; }

        public int Pressure { get; set; }

        public int Clouds { get; set; }
    }
}
