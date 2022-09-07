using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CurrentCityCangedArgs
    {
        public CurrentCityCangedArgs(CurrentWeather current)
        {
            CurrentWeather = current;
        }

        public CurrentWeather CurrentWeather { get; }
    }
}
