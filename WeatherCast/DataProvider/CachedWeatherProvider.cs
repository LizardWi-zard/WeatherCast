using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;

        public CachedWeatherProvider()
        {
            internetDataProvider = new InternetWeatherProvider();
            fileDataProvider = new FileWeatherProvider();
        }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            throw new System.NotImplementedException();
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            throw new System.NotImplementedException();
        }
    }
}
