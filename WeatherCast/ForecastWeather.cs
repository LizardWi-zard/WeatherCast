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
    }

    public class DailyCast
    {
        public int Dt { get; set; }

        public DateTime Date { get; set; }

        public FutureTempInfo Temp { get; set; }

        public FutureFeelsLike Feels_Like { get; set; }

        public int Pressure { get; set; }

        public FutureWeatherInfo[] Weather { get; set; }

        public int Clouds { get; set; }

        public int TempToInt(float temp)
        {
            int tmp = (int)temp;

            return tmp;
        }

        public DateTime GetDate(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
    public class FutureTempInfo
    {
        public float Day { get; set; }

        public int DayInt { get; set; }

        public float Min { get; set; }

        public float Max { get; set; }

        public float Night { get; set; }     
    }

    public class FutureFeelsLike
    {
        public float Day { get; set; }

        public float Night { get; set; }
    }

    public class FutureWeatherInfo
    {
        public string Main { get; set; }

        public string Description { get; set; }
    }
}
