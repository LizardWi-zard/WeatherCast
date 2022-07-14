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

        private const string defaultCity = "Москва";
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
            if (TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo))
            {
                CurrentWeather weather;

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    weather = internetDataProvider.GetCurrentWeather(selectedCity);

                    SaveCurrentWeatherData(weather);
                    SaveLastRequestInfo(lastRequestInfo);
                }
                else
                {
                    weather = fileDataProvider.GetCurrentWeather(selectedCity);
                }

                return weather;
            }

            return CurrentWeather.Empty;
        }

        // пофиксь по агалогии с GetCurrentWeather
        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            List<string> fileData = TryGetCityNameAndRequestTime();

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

        private bool TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo)
        {
            FileInfo requestTimeFileInfo = new FileInfo(Definitions.RequestTimePath);
            string cityName;
            DateTime lastRequestTime;

            string[] fileStrings = new string[2];

            if (requestTimeFileInfo.Exists)
            {
                fileStrings = File.ReadAllLines(Definitions.RequestTimePath);
                cityName = fileStrings[0];
                lastRequestTime = DateTime.Parse(fileStrings[1]);
            }
            else
            {
                lastRequestInfo = new LastRequestInfo(defaultCity, DateTime.MinValue);
                return false;
            }

            lastRequestInfo = new LastRequestInfo(cityName, lastRequestTime);
            return true;
        }

        private void SaveCurrentWeatherData(CurrentWeather weather) 
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.SelectedCityCurrenWeatherInfoPath));
            CreateIfNotExist(Definitions.SelectedCityCurrenWeatherInfoPath);

            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, weather);
                sw.Close();
            }
        }

        private void SaveLastRequestInfo(LastRequestInfo lastRequestInfo)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.RequestTimePath));
            CreateIfNotExist(Definitions.RequestTimePath);

            File.AppendAllLines(Definitions.RequestTimePath, lastRequestInfo.ToArray());
        }

        private void OverWriteFutureWeatherData(ForecastWeather weather)
        {
            if (!File.Exists(Definitions.SelectedCityFutureWeatherInfoPath))
            {
                File.Create(Definitions.SelectedCityFutureWeatherInfoPath).Close();
            }

            var data = weather;

            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityFutureWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, data);
                sw.Close();
            }
        }

        private void CreateIfNotExist(string path)
        {
            FileAttributes fileSystemItemattributes = File.GetAttributes(path);

            var isDirectory = (fileSystemItemattributes & FileAttributes.Directory) == FileAttributes.Directory;

            if (isDirectory)
            {

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            File.Create(path).Close();
            return;
        }
    }
}
