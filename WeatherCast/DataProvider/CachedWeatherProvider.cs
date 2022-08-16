using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
        
        public CachedWeatherProvider(IDataProvider internetDataProvider, IDataProvider fileDataProvider, TimeSpan upadteCacheInterval)
        {
            this.internetDataProvider = internetDataProvider ?? throw new ArgumentNullException(nameof(internetDataProvider));
            this.fileDataProvider = fileDataProvider ?? throw new ArgumentNullException(nameof(fileDataProvider));
            this.upadteCacheInterval = upadteCacheInterval;

            timer.Interval = upadteCacheInterval.TotalMilliseconds;
            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;
            timer.Start();

            if (File.Exists(Definitions.DirectoryPath)) //TODO: find why something creating directory as file
            {
                File.Delete(Definitions.DirectoryPath);
            }

            if (!Directory.Exists(Definitions.DirectoryPath))
            {
                Directory.CreateDirectory(Definitions.DirectoryPath);
            }
        }

        public delegate void OnWeatherUpdated(object? sender, WeatherUpdatedEventArgs? e);

        public event OnWeatherUpdated OnWeatherAutoUpdate;

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            Validate.CityName(cityName, nameof(cityName));

            CurrentWeather weather;
            DateTime lastRequestTime = DateTime.Now;
            SettingsProvider settings = SettingsProvider.getInstance();

            string fileCityName = null;

            if (TryGetCurrentCityNameAndRequestTime(out LastRequestCurrentInfo lastRequestCurrentInfo))
            {
                lastRequestTime = lastRequestCurrentInfo.RequestTime;

                fileCityName = lastRequestCurrentInfo.CityName.ToLower();

                if (settings.FirstLaunch == true)
                {
                    cityName = fileCityName;

                    settings.FirstLaunch = false;
                }
            }
            else
            {
                lastRequestTime = settings.LastRequestTime;
            }

            var difference = DateTime.Now.Subtract(lastRequestTime);
            if (difference >= upadteCacheInterval)
            {
                try
                {
                    weather = internetDataProvider.GetCurrentWeather(cityName);

                    SaveCurrentWeatherData(weather);

                    lastRequestCurrentInfo = new LastRequestCurrentInfo()
                    {
                        CityName = cityName,
                        RequestTime = DateTime.Now
                    };
                    
                    SaveLastCurrentRequestInfo(lastRequestCurrentInfo);
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
                    weather = fileDataProvider.GetCurrentWeather(cityName);
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
            SettingsProvider settings = SettingsProvider.getInstance();

            if (TryGetForecastCityNameAndRequestTime(out LastRequestForecastInfo lastRequestInfo))
            {
                lastRequestTime = lastRequestInfo.RequestTime;
            }
            else
            {
                lastRequestTime = settings.LastRequestTime;
            }

            var difference = DateTime.Now.Subtract(lastRequestTime);
            if (difference >= upadteCacheInterval)
            {
                try
                {
                    weather = internetDataProvider.GetForecastWeather(longitude, latitude);

                    SaveFutureWeatherData(weather);

                    lastRequestInfo = new LastRequestForecastInfo()
                    {
                        Longitude = longitude,
                        Latitude = latitude,
                        RequestTime = DateTime.Now
                    };

                    SaveLastForecastRequestInfo(lastRequestInfo);
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

        public List<CurrentWeather> GetMarkedCitiesCurrentWeather(List<string> markedCitiesNames)  //TODO: refactoring
        {
            DateTime lastRequestTime = DateTime.Now;
            SettingsProvider settings = SettingsProvider.getInstance();

            if (TryGetMarkedCityCurrentWeather(out List<CurrentWeather> markedCitiesWeather))
            {
                if( markedCitiesNames.Count() == 0)
                {
                    UpdateMarkedCitiesNames(markedCitiesWeather);

                    return markedCitiesWeather;
                }
            }

            var updatedWeatherInfo = new List<CurrentWeather>();

            var difference = DateTime.Now.Subtract(settings.LastRequestTime);
            
            foreach(string item in markedCitiesNames)
            {
                Validate.CityName(item, nameof(item));

                updatedWeatherInfo.Add(internetDataProvider.GetCurrentWeather(item));
            }

            SaveMarkedCitiesCurrentWeatherInfo(updatedWeatherInfo);

            return updatedWeatherInfo;
        }

        private CurrentWeather GetMarkedCity(string name)
        {
            CurrentWeather[] markedCities;

            using (StreamReader sr = new StreamReader(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                JsonTextReader reader = new JsonTextReader(sr);
                markedCities = new JsonSerializer().Deserialize<CurrentWeather[]>(reader);
            }

            foreach( var item in markedCities)
            {
                if(item.Name.ToLower() == name)
                {
                    return item;
                }
            }

            return null;
        }

        private void UpdateMarkedCitiesNames(List<CurrentWeather> Names)
        {
            SettingsProvider singleton = SettingsProvider.getInstance();

            foreach(var item in Names)
            {
                singleton.AddCity(item.Name);
            }
        }

        private void OnTimedEvent(object sourse, System.Timers.ElapsedEventArgs e)
        {
            TryGetCurrentCityNameAndRequestTime(out LastRequestCurrentInfo lastRequestInfo);

            var current = GetCurrentWeather(lastRequestInfo.CityName);

            var longitude = current.Coord.Longitude.ToString();
            var latitude = current.Coord.Latitude.ToString();

            var forecast = GetForecastWeather(longitude, latitude);

            var args = new WeatherUpdatedEventArgs(current, forecast);

            OnWeatherAutoUpdate?.Invoke(this, args);
        }

        private bool TryGetCurrentCityNameAndRequestTime(out LastRequestCurrentInfo lastRequestInfo)
        {
            if (File.Exists(Definitions.RequestCurrentInfoTimePath))
            {
                using (StreamReader sr = new StreamReader(Definitions.RequestCurrentInfoTimePath))
                {
                    JsonTextReader reader = new JsonTextReader(sr);
                    lastRequestInfo = new JsonSerializer().Deserialize<LastRequestCurrentInfo>(reader);
                }
            }
            else
            {
                lastRequestInfo = new LastRequestCurrentInfo();
                return false;
            }

            return true;
        }

        private bool TryGetForecastCityNameAndRequestTime(out LastRequestForecastInfo lastRequestInfo)
        {
            if (File.Exists(Definitions.RequestFutureInfoTimePath))
            {
                using (StreamReader sr = new StreamReader(Definitions.RequestFutureInfoTimePath))
                {
                    JsonTextReader reader = new JsonTextReader(sr);
                    lastRequestInfo = new JsonSerializer().Deserialize<LastRequestForecastInfo>(reader);
                }
            }
            else
            {
                lastRequestInfo = new LastRequestForecastInfo();
                return false;
            }

            return true;
        }

        private bool TryGetMarkedCityCurrentWeather(out List<CurrentWeather> markedCitiesWeather)
        {
            if (File.Exists(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                using (StreamReader sr = new StreamReader(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
                {
                    JsonTextReader reader = new JsonTextReader(sr);
                    markedCitiesWeather = new JsonSerializer().Deserialize<List<CurrentWeather>>(reader);
                }
            }
            else
            {
                markedCitiesWeather = new List<CurrentWeather>();
                return false;
            }

            return true;
        }

        private void SaveCurrentWeatherData(CurrentWeather weather) 
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.SelectedCityCurrenWeatherInfoPath));
           
            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, weather);
                sw.Close();
            }
        }

        private void SaveLastCurrentRequestInfo(LastRequestCurrentInfo lastRequestInfo)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.RequestCurrentInfoTimePath));
            
            using (StreamWriter sw = File.CreateText(Definitions.RequestCurrentInfoTimePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, lastRequestInfo);
                sw.Close();
            }
        }

        private void SaveFutureWeatherData(ForecastWeather weather)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.SelectedCityFutureWeatherInfoPath));
           
            using (StreamWriter sw = File.CreateText(Definitions.SelectedCityFutureWeatherInfoPath)) // StreamWriter не нуждается в создании пустого файла (создаёт сам)
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, weather);
                sw.Close();
            }
        }

        private void SaveLastForecastRequestInfo(LastRequestForecastInfo lastRequestInfo)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.RequestFutureInfoTimePath));

            using (StreamWriter sw = File.CreateText(Definitions.RequestFutureInfoTimePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, lastRequestInfo);
                sw.Close();
            }
        }

        private void SaveMarkedCitiesCurrentWeatherInfo(List<CurrentWeather> weather)
        {
            CreateIfNotExist(Path.GetDirectoryName(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath));

            using (StreamWriter sw = File.CreateText(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, weather);
                sw.Close();
            }
        }

        private void CreateIfNotExist(string path)
        {
            FileAttributes fileSystemItemattributes = File.GetAttributes(path);

            //var isDirectory = (fileSystemItemattributes & FileAttributes.Directory) == FileAttributes.Directory;

            if (fileSystemItemattributes == FileAttributes.Directory)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            
                return;
            }

            File.Create(path).Close();
        }
    }
}
