using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCast.DataProvider;
using WeatherCast.Model;

namespace WeatherCast.Tests.Mocks
{
    internal class MockDataProvider : IDataProvider
    {
        public bool GetCurrentWeatherIsCalled { get; private set; }

        public bool GetForecastWeatherIsCalled { get; private set; }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            GetCurrentWeatherIsCalled = true;

            return CurrentWeather.Empty;
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            GetForecastWeatherIsCalled = true;

            return ForecastWeather.Empty;
        }
    }
}
