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
        private string url = "http://api.openweathermap.org/data/2.5/weather?q=Ivanovo&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";

        private HttpWebRequest httpWebRequest { get; set; }

        private HttpWebResponse httpWebResponse { get; set; }

        public string Response { get; set; }

        private void WebRequestResponse()
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public WeatherResponse GetResponse()
        {
            WebRequestResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                Response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(Response);

            return weatherResponse;
        }

    }

    public class WeatherResponse
    {
        public string Name { get; set; }

        public TemperatureInfo Main { get; set; }
    }

    public class TemperatureInfo
    {
        public float Temp { get; set; }

        public float Feels_Like { get; set; }
    }
}
