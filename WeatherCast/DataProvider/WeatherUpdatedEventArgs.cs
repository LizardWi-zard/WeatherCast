using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class WeatherUpdatedEventArgs
    {
        public WeatherUpdatedEventArgs(CurrentWeather current, ForecastWeather forecast)
        {
            CurrentWeather = current;
            ForecastWeather = forecast;
        }

        public CurrentWeather CurrentWeather { get; }

        public ForecastWeather ForecastWeather { get; }
    }
}
