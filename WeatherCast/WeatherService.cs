﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using WeatherCast.Model;

namespace WeatherCast
{
    public class WeatherService
    {
        private HttpWebResponse httpWebResponse;

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException();
            }

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

            forecastWeather.ForecastFor24Hours = forecastWeather.Hourly.Take(24);

            return forecastWeather;
        }

        private string CreateСurrentWeatherUrl(string cityName)
        {
            return "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&lang=ru&appid=894e6bd5a5ce67550af68bcd05ead93d";
        }

        private string CreateFutureWeatherUrl(string lon, string lat)
        {
            return $"https://api.openweathermap.org/data/2.5/onecall?lon={lon}&lat={lat}&units=metric&lang=ru&appid=894e6bd5a5ce67550af68bcd05ead93d";
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