using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;

        private const string homeCity = "Москва";
        private string selectedCity = "Москва";
        private DateTime lastRequestTime = DateTime.Now;

        public CachedWeatherProvider()
        {
            internetDataProvider = new InternetWeatherProvider();
            fileDataProvider = new FileWeatherProvider();
        }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            List<string> fileData = GetCityAndRequestTime();

            selectedCity = fileData[0];
            lastRequestTime = DateTime.Parse(fileData[1]);

            if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
            {
                return internetDataProvider.GetCurrentWeather(selectedCity);
            }
            else
            {
                return fileDataProvider.GetCurrentWeather(selectedCity);
            }
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            throw new System.NotImplementedException();
        }

        private List<string> GetCityAndRequestTime()
        {
            FileInfo fileInf = new FileInfo(Definitions.RequestTimePath);

            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(Definitions.RequestTimePath).ToList();
                DateTime lastRequestTime = DateTime.Parse(arrLine[1]);
                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                }

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);

                return arrLine;
            }
            else
            {
                arrLine.Add(homeCity);
                arrLine.Add(DateTime.Now.ToString());

                // TODO добавить проверку на существование папки, при первом запуске её нет и она в этом случае никем не создаётся

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);

                return arrLine;
            }
        }
    }
}
