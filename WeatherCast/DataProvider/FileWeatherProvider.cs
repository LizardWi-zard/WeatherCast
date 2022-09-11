using Newtonsoft.Json;
using System.IO;
using WeatherCast.Helpers;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class FileWeatherProvider : IDataProvider
    {
        public CurrentWeather GetCurrentWeather(string cityName)
        {
            Validate.CityName(cityName, nameof(cityName));

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

        public ForecastWeather GetForecastWeather(string lon, string lat, bool isPlannedRequest)
        {
            Validate.GeographicCoordinateValue(lon, "longitude");
            Validate.GeographicCoordinateValue(lat, "latitude");

            double longitude = double.Parse(lon);
            double latitude = double.Parse(lat);

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