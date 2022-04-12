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

        string url;

        public void CreateAPIurl(string str)
        {
            this.url = "http://api.openweathermap.org/data/2.5/weather?q=" + str + "&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";
        }

        public WeatherResponse GetResponse()
        {
            if (WebRequestResponse() != null)
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                Console.WriteLine();

                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                return weatherResponse;
            }

            return null;
        }

        private HttpWebResponse WebRequestResponse()
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);



            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return httpWebResponse;

        }
    }

    public class WeatherResponse
    {
        public string Name { get; set; }

        public TemperatureInfo Main { get; set; }

        public WeatherInfo[] Weather { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; }

        public string Description { get; set; }
    }

    public class TemperatureInfo
    {
        public float Temp { get; set; }

        public float Feels_Like { get; set; }
    }
}
