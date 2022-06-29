using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class FutureFeelsLike
    {
        [JsonProperty("day")]
        public float DayTemperature { get; set; }

        [JsonProperty("night")]
        public float NightTemperature { get; set; }
    }
}
