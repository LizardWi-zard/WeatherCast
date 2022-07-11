using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    public interface IDataProvider
    {
        CurrentWeather GetCurrentWeather(string cityName);

        ForecastWeather GetForecastWeather(string longitude, string latitude);
    }
}
