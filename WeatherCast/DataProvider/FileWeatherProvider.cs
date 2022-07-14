using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class FileWeatherProvider : IDataProvider
    {
        public CurrentWeather GetCurrentWeather(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName) || !(cityName.All(c => Char.IsLetter(c) || c == '-') && cityName.Count(f => f == '-') < 2 && cityName.Length > 1))
            {
                throw new ArgumentException();
            }

            if (!File.Exists(Definitions.SelectedCityCurrenWeatherInfoPath))
            {
                throw new FileNotFoundException();
            }

            string fileData;

            using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityCurrenWeatherInfoPath))
            {
                fileData = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(fileData);

            return currentWeather;
        }

        public ForecastWeather GetForecastWeather(string lon, string lat)
        {
            double latitude;
            double longitude;

            if (!double.TryParse(lon, out longitude) || !double.TryParse(lat, out latitude))
            {
                throw new ArgumentException();
            }

            if (!File.Exists(Definitions.SelectedCityFutureWeatherInfoPath))
            {
                throw new FileNotFoundException();
            }

            string fileData;

            using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityFutureWeatherInfoPath))
            {
                fileData = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var futureWeather = JsonConvert.DeserializeObject<ForecastWeather>(fileData);

            return futureWeather;
        }
    }
}