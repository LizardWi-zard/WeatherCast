using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class DailyCast
    {
        [JsonProperty("dt")]
        public int Date { get; set; }

        public int Sunrise { get; set; }

        public int Sunset { get; set; }

        public int Moonrise { get; set; }

        public int Moonset { get; set; }

        [JsonProperty("temp")]
        public FutureTempInfo Temperature { get; set; }

        public FutureFeelsLike Feels_Like { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public float Wind_Speed { get; set; }

        public FutureWeatherInfo[] Weather { get; set; }

        public int Clouds { get; set; }

        [JsonProperty("pop")]
        public float ProbabilityOfPrecipitation { get; set; }

        [JsonProperty("uvi")]
        public float UVindex { get; set; }
    }
}
