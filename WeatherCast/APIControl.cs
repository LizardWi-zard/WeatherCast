using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast
{
    public class APIControl
    {
        public HttpWebRequest httpWebRequest { get; set; }

        public HttpWebResponse httpWebResponse { get; set; }

        string response;

        string currentWeatherUrl;
        string futureWeatherUrl;

        public void CreateСurrentWeatherUrl(string cityName)
        {
            currentWeatherUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";
        }

        public void CreateFutureWeatherUrl(string lon, string lat)
        {
            futureWeatherUrl = $"https://api.openweathermap.org/data/2.5/onecall?lon={lon}&lat={lat}&lang=ru&appid=8b946297edc5dc36bd60f3acab86dc68";
        }


        //TODO: combine methods by making switch 
        public CurrentWeather CurrentWeather()
        {
            CurrentWeather currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(GetResponseAsString(currentWeatherUrl));

            return currentWeather;
        }

        public ForecastWeather FutureWeather()
        {
            ForecastWeather futureWeather = JsonConvert.DeserializeObject<ForecastWeather>(GetResponseAsString(futureWeatherUrl));

            return futureWeather;
        }

        public string GetResponseAsString(string geivenUrl)
        {
            if (WebRequestResponse(geivenUrl) != null)
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                return response;
            }

            return null;
        }

        private HttpWebResponse WebRequestResponse(string geivenUrl)
        {
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(geivenUrl);

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return httpWebResponse;

        }
    }
}
