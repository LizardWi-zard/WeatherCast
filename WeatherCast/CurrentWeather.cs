using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast
{
    public class CurrentWeather
    {
        public string Name { get; set; }

        public Coordinates Coord { get; set; }

        public CurrentWeatherInfo[] Weather { get; set; }

        public CurrentTempInfo Main { get; set; }
    }

    public class CurrentTempInfo
    {
        public float Temp { get; set; }

        public float Feels_Like { get; set; }
    }

    public class CurrentWeatherInfo
    {
        public string Main { get; set; }

        public string Description { get; set; }
    }

    public class Coordinates
    {
        public float Lon { get; set; }

        public float Lat { get; set; }
    }
}
