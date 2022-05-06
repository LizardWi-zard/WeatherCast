using Newtonsoft.Json;
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

        public HourCast[] Hourly { get; set; }

        public IEnumerable<HourCast> ForecastFor24Hours { get; set; }
    }

    public class DailyCast
    {
        public int Dt { get; set; }

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

    public class HourCast
    {
        public int Dt { get; set; }

        [JsonProperty("temp")]
        public float Temperature { get; set; }

        public float Feels_Like { get; set; }

        public int Pressure { get; set; }

        public int Clouds { get; set; }

        public float Wind_Speed { get; set; }

        public FutureWeatherInfo[] Weather { get; set; }
    }

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

    public class FutureFeelsLike
    {
        [JsonProperty("day")]
        public float DayTemperature { get; set; }

        [JsonProperty("night")]
        public float NightTemperature { get; set; }
    }

    public class FutureWeatherInfo
    {
        public string Main { get; set; }

        public string Description { get; set; }
    }
}
