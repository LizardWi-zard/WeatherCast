using System;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class FileWeatherProvider : IDataProvider
    {
        public CurrentWeather GetCurrentWeather(string cityName)
        {
            throw new NotImplementedException();
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            throw new NotImplementedException();
        }
    }
}
