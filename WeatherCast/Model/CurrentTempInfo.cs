using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class CurrentTempInfo
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        public float Feels_Like { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }
}
