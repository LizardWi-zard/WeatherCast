using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class HourCast
    {
        [JsonProperty("dt")]
        public int Date { get; set; }

        [JsonProperty("temp")]
        public float Temperature { get; set; }

        public float Feels_Like { get; set; }

        public int Pressure { get; set; }

        public int Clouds { get; set; }

        public float Wind_Speed { get; set; }

        public FutureWeatherInfo[] Weather { get; set; }
    }
}
