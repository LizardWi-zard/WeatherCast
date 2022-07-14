using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;
        private Timer timer = new Timer();

        private const string homeCity = "Москва";
        private string selectedCity = "Москва";
        private DateTime lastRequestTime = DateTime.Now;

        public CachedWeatherProvider()
        {
            internetDataProvider = new InternetWeatherProvider();
            fileDataProvider = new FileWeatherProvider();

            timer.Interval = 1000 * 60 * 30;
            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;
            timer.Start();

            if (!Directory.Exists(Definitions.DirectoryPath))
            {
                Directory.CreateDirectory(Definitions.DirectoryPath);
            }
        }

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            List<string> fileData = GetCityAndRequestTime();

            selectedCity = fileData[0];
            lastRequestTime = DateTime.Parse(fileData[1]);

            CurrentWeather weather;

            if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
            {
                weather = internetDataProvider.GetCurrentWeather(selectedCity);

                OverWriteCurrentWeatherData(weather);
            }
            else
            {
                weather =  fileDataProvider.GetCurrentWeather(selectedCity);
            }

            return weather;
        }

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            List<string> fileData = GetCityAndRequestTime();

            selectedCity = fileData[0];
            lastRequestTime = DateTime.Parse(fileData[1]);

            ForecastWeather weather;

            if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
            {
                weather = internetDataProvider.GetForecastWeather(longitude, latitude);

                OverWriteFutureWeatherData(weather);
            }
            else
            {
                weather = fileDataProvider.GetForecastWeather(longitude, latitude);
            }

            return weather;
        }

        private void OnTimedEvent(Object sourse, System.Timers.ElapsedEventArgs e)
        {

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

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);

                return arrLine;
            }
        }

        public void OverWriteCurrentWeatherData(CurrentWeather weather) 
        {
            if (!File.Exists(Definitions.SelectedCityCurrentInfoPath))
            {
                File.Create(Definitions.SelectedCityCurrentInfoPath).Close();
            }

            var data = weather;

            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrentInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, data);
                sw.Close();
            }
        }

        public void OverWriteFutureWeatherData(ForecastWeather weather)
        {
            if (!File.Exists(Definitions.SelectedCityFutureInfoPath))
            {
                File.Create(Definitions.SelectedCityFutureInfoPath).Close();
            }

            var data = weather;

            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityFutureInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, data);
                sw.Close();
            }
        }
    }
}
