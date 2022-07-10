using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal interface IDataProvider
    {
        CurrentWeather GetCurrentWeather(string cityName);

        ForecastWeather GetForecastWeather(string longitude, string latitude);
    }
}
