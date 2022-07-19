using WeatherCast.DataProvider;
using WeatherCast.Model;

namespace WeatherCast.Tests.Mocks
{
    internal class MockDataProvider : IDataProvider
    {
        public bool GetCurrentWeatherWasCalled { get; private set; }

        public bool GetForecastWeatherWasCalled { get; private set; }

        public CurrentWeather ReturnForGetCurrentWeather { get; set; }

        public ForecastWeather ReturnForForecastWeather { get; set; }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            GetCurrentWeatherWasCalled = true;

            return ReturnForGetCurrentWeather;
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            GetForecastWeatherWasCalled = true;

            return ReturnForForecastWeather;
        }
    }
}
