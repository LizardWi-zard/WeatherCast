using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast
{
    public class WeatherService
    {
        private HttpWebRequest httpWebRequest;
        private HttpWebResponse httpWebResponse;

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            var requestUrl = CreateСurrentWeatherUrl(cityName);

            var response = GetResponseAsString(requestUrl);

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(response);

            return currentWeather;
        }

        public ForecastWeather GetForecastWeather(string lon, string lat)
        {
            var requestUrl = CreateFutureWeatherUrl(lon, lat);

            var response = GetResponseAsString(requestUrl);

            var forecastWeather = JsonConvert.DeserializeObject<ForecastWeather>(response);

            var refactoredForecastWeather = GetRefactoredDataForFurutureWeather(forecastWeather);

            return refactoredForecastWeather;
        }

        private string CreateСurrentWeatherUrl(string cityName)
        {
            return "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&lang=ru&appid=8b946297edc5dc36bd60f3acab86dc68";
        }

        private string CreateFutureWeatherUrl(string lon, string lat)
        {
            return $"https://api.openweathermap.org/data/2.5/onecall?lon={lon}&lat={lat}&units=metric&lang=ru&appid=8b946297edc5dc36bd60f3acab86dc68";
        }


        private string GetResponseAsString(string givenUrl)
        {
            string response;

            if (WebRequestResponse(givenUrl) != null)
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                return response;
            }

            return null;
        }

        private HttpWebResponse WebRequestResponse(string givenUrl)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(givenUrl);

                return httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private ForecastWeather GetRefactoredDataForFurutureWeather(ForecastWeather response)
        {
            ForecastWeather forecastWeather = response;

            foreach (var day in forecastWeather.Daily)
            {
                day.Date = forecastWeather.GetDate(day.Dt);
                day.Temperature.DayTemperature = forecastWeather.TempToInt(day.Temperature.DayTemperature);
                day.Temperature.NightTemperature = forecastWeather.TempToInt(day.Temperature.NightTemperature);
            }


            forecastWeather.ForecastFor24Hours = new List<HourCast>();

            for (int i = 0; i < 24; i++)
            {
                forecastWeather.ForecastFor24Hours.Add(forecastWeather.Hourly[i]);
            }

            foreach (var hour in forecastWeather.ForecastFor24Hours)
            {
                hour.Date = forecastWeather.GetDate(hour.Dt);
                hour.Temperature = forecastWeather.TempToInt(hour.Temperature);
                hour.Feels_Like = forecastWeather.TempToInt(hour.Feels_Like);
            }

            return forecastWeather;
        }
    }
}