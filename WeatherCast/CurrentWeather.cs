using Newtonsoft.Json;
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

        public WindInfo Wind { get; set; }
    }

    public class CurrentTempInfo
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        public float Feels_Like { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }

    public class WindInfo
    {
        public float Speed { get; set; }

        [JsonProperty("deg")]
        public int Degree { get; set; }
    }

    public class CurrentWeatherInfo
    {
        public string Main { get; set; }

        public string Description { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }
}
