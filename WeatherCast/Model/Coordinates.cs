using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class Coordinates
    {
        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }
}
