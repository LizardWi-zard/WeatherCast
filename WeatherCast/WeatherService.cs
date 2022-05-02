﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            return forecastWeather;
        }

        private string CreateСurrentWeatherUrl(string cityName)
        {
            return "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";
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
    }
}