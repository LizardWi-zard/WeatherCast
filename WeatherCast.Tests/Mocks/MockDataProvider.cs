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
        public bool GetCurrentWeatherWasCalled { get; private set; }

        public bool GetForecastWeatherWasCalled { get; private set; }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            GetCurrentWeatherWasCalled = true;

            return CurrentWeather.Empty;
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            GetForecastWeatherWasCalled = true;

            return ForecastWeather.Empty;
        }
    }
}
