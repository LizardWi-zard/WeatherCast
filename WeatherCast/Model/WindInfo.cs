using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class WindInfo
    {
        public float Speed { get; set; }

        [JsonProperty("deg")]
        public int Degree { get; set; }
    }
}
