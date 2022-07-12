using Newtonsoft.Json;
using System;
using System.IO;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class FileWeatherProvider : IDataProvider
    {
        public CurrentWeather GetCurrentWeather(string cityName)
        {
            string fileData;

            using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityCurrentInfoPath))
            {
                fileData = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(fileData);

            return currentWeather;
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
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