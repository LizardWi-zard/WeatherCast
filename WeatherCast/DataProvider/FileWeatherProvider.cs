using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class FileWeatherProvider : IDataProvider
    {
        public CurrentWeather GetCurrentWeather(string cityName)
        {
            Regex rgx = new Regex(@"\p{Cs}");

            if (string.IsNullOrWhiteSpace(cityName) || rgx.IsMatch(cityName))
            {
                throw new ArgumentException();
            }

            string fileData;

            using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityCurrentInfoPath))
            {
                fileData = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(fileData);

            return currentWeather;
        }

        public ForecastWeather GetForecastWeather(string lon, string lat)
        {
            Regex rgx = new Regex(@"\p{Cs}");

            if (string.IsNullOrWhiteSpace(lon) || string.IsNullOrWhiteSpace(lat) ||
                rgx.IsMatch(lon) || rgx.IsMatch(lat))
            {
                throw new ArgumentException();
            }

            double latitude;
            double longitude;

            if (!double.TryParse(lon, out longitude) || !double.TryParse(lat, out latitude))
            {
                throw new ArgumentException();
            }

            string fileData;

            using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityFutureInfoPath))
            {
                fileData = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var futureWeather = JsonConvert.DeserializeObject<ForecastWeather>(fileData);

            return futureWeather;
        }
    }
}