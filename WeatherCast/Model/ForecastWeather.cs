using System.Collections.Generic;

namespace WeatherCast.Model
{
    public class ForecastWeather
    {
        public float Lat { get; set; }

        public float Lon { get; set; }

        public string TimeZone { get; set; }

        public DailyCast[] Daily { get; set; }

        public HourCast[] Hourly { get; set; }

        public IEnumerable<HourCast> ForecastFor24Hours { get; set; }

        public static ForecastWeather Empty { get; } = new ForecastWeather()
        {
            Daily = new DailyCast[] 
            { 
                new DailyCast() 
                { 
                    Temperature = new FutureTempInfo(), 
                    Feels_Like = new FutureFeelsLike()
                }
            },
            
            Hourly = new HourCast[]
            {
                new HourCast()
                {
                    Weather = new FutureWeatherInfo[0]
                }
            } ,
            
            ForecastFor24Hours = new HourCast[]
            {
                new HourCast()
                {
                    Weather = new FutureWeatherInfo[0]
                }
            }
        };
    }
}
