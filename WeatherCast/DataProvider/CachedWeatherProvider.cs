using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;

        public CachedWeatherProvider(IDataProvider internetDataProvider, IDataProvider fileDataProvider)
        {
            Guard.NotNull(internetDataProvider, nameof(internetDataProvider));
            Guard.NotNull(fileDataProvider, nameof(fileDataProvider));

            this.internetDataProvider = internetDataProvider;
            this.fileDataProvider = fileDataProvider;
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
