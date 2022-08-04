using Newtonsoft.Json;

namespace WeatherCast.Model
{
    public class FutureTempInfo
    {
        [JsonProperty("day")]
        public float DayTemperature { get; set; }

        [JsonProperty("min")]
        public float MinTemperature { get; set; }

        public string MinTemperatureText { get; set; }

        [JsonProperty("max")]
        public float MaxTemperature { get; set; }

        public string MaxTemperatureText { get; set; }

        [JsonProperty("night")]
        public float NightTemperature { get; set; }
    }
}
