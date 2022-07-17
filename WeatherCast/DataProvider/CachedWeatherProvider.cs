using Newtonsoft.Json;
using System;
using System.IO;
using System.Timers;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;
        private readonly TimeSpan upadteCacheInterval;
        private Timer timer = new Timer();

        private const string defaultCity = "Москва";
        private string selectedCity = "Москва";

        public CachedWeatherProvider(IDataProvider internetDataProvider, IDataProvider fileDataProvider, TimeSpan upadteCacheInterval)
        {
            this.internetDataProvider = internetDataProvider ?? throw new ArgumentNullException(nameof(internetDataProvider));
            this.fileDataProvider = fileDataProvider ?? throw new ArgumentNullException(nameof(fileDataProvider));
            this.upadteCacheInterval = upadteCacheInterval;

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
            CurrentWeather weather;
            DateTime lastRequestTime = DateTime.Now;

            if (TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo))
            {
                if (lastRequestInfo.CityName == cityName)
                {
                    lastRequestTime = lastRequestInfo.RequestTime;
                }
            }

            if (DateTime.Now.Subtract(lastRequestTime) >= upadteCacheInterval)
            {
                try
                {
                    weather = internetDataProvider.GetCurrentWeather(selectedCity);

                    SaveCurrentWeatherData(weather);
                    SaveLastRequestInfo(new LastRequestInfo(cityName, DateTime.Now));
                }
                catch
                {
                    // some logging
                    weather = CurrentWeather.Empty;
                }
            }
            else
            {
                try
                {
                    weather = fileDataProvider.GetCurrentWeather(selectedCity);
                }
                catch
                {
                    // some logging
                    weather = CurrentWeather.Empty;
                }
            }

            return weather;
        }

        // TODO: need implement
        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            throw new NotImplementedException();
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

        private void SaveFutureWeatherData(ForecastWeather weather)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.SelectedCityFutureWeatherInfoPath));
            CreateIfNotExist(Definitions.SelectedCityFutureWeatherInfoPath);

            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, weather);
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
