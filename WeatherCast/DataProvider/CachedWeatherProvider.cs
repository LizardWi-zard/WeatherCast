using Newtonsoft.Json;
using System;
using System.IO;
using System.Timers;
using WeatherCast.Helpers;
using WeatherCast.Model;

namespace WeatherCast.DataProvider
{
    internal class CachedWeatherProvider : IDataProvider
    {
        private readonly IDataProvider internetDataProvider;
        private readonly IDataProvider fileDataProvider;
        private readonly TimeSpan upadteCacheInterval;
        private Timer timer = new Timer();
        private string selectedCity = "Москва";

        public delegate void OnWeatherWasUpdated(object? sender, EventArgs? e);

        public event OnWeatherWasUpdated WeatherWasUpdated;

        public CachedWeatherProvider(IDataProvider internetDataProvider, IDataProvider fileDataProvider, TimeSpan upadteCacheInterval)
        {
            this.internetDataProvider = internetDataProvider ?? throw new ArgumentNullException(nameof(internetDataProvider));
            this.fileDataProvider = fileDataProvider ?? throw new ArgumentNullException(nameof(fileDataProvider));
            this.upadteCacheInterval = upadteCacheInterval;

            timer.Interval = upadteCacheInterval.TotalMilliseconds;
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
            Validate.CityName(cityName, nameof(cityName));

            CurrentWeather weather;
            DateTime lastRequestTime = DateTime.Now;

            if (TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo))
            {
                if (lastRequestInfo.CityName == cityName)
                {
                    lastRequestTime = lastRequestInfo.RequestTime;
                }
            }

            var difference = DateTime.Now.Subtract(lastRequestTime);
            if (difference >= upadteCacheInterval)
            {
                try
                {
                    weather = internetDataProvider.GetCurrentWeather(selectedCity);

                    SaveCurrentWeatherData(weather);

                    lastRequestInfo = new LastRequestInfo()
                    {
                        CityName = cityName,
                        RequestTime = DateTime.Now
                    };
                    
                    SaveLastRequestInfo(lastRequestInfo);
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

        public ForecastWeather GetForecastWeather(string longitude, string latitude)
        {
            Validate.GeographicCoordinateValue(longitude, "longitude");
            Validate.GeographicCoordinateValue(latitude, "latitude");

            ForecastWeather weather;
            DateTime lastRequestTime = DateTime.Now;

            if (TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo))
            {
                if (lastRequestInfo.Longitude == longitude && lastRequestInfo.Latitude == latitude)
                {
                    lastRequestTime = lastRequestInfo.RequestTime;
                }
            }

            if (DateTime.Now.Subtract(lastRequestTime) >= upadteCacheInterval)
            {
                try
                {
                    weather = internetDataProvider.GetForecastWeather(longitude, latitude);

                    SaveFutureWeatherData(weather);

                    lastRequestInfo = new LastRequestInfo()
                    {
                        Longitude = longitude,
                        Latitude = latitude
                    };

                    SaveLastRequestInfo(lastRequestInfo);
                }
                catch
                {
                    // some logging
                    weather = ForecastWeather.Empty;
                }
            }
            else
            {
                try
                {
                    weather = fileDataProvider.GetForecastWeather(longitude, latitude);
                }
                catch
                {
                    // some logging
                    weather = ForecastWeather.Empty;
                }
            }

            return weather;
        }

        private void OnTimedEvent(object sourse, ElapsedEventArgs e)
        {
            TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo);

            GetCurrentWeather(lastRequestInfo.CityName);
            GetForecastWeather(lastRequestInfo.Longitude, lastRequestInfo.Latitude);

            WeatherWasUpdated?.Invoke(null, null);
        }

        private bool TryGetCityNameAndRequestTime(out LastRequestInfo lastRequestInfo)
        {
            if (File.Exists(Definitions.RequestTimePath))
            {
                using (StreamReader sr = new StreamReader(Definitions.RequestTimePath))
                {
                    JsonTextReader reader = new JsonTextReader(sr);
                    lastRequestInfo = new JsonSerializer().Deserialize<LastRequestInfo>(reader);
                }
            }
            else
            {
                lastRequestInfo = new LastRequestInfo();
                return false;
            }

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

            using (StreamWriter sw = File.CreateText(Definitions.RequestTimePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, lastRequestInfo);
                sw.Close();
            }
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
