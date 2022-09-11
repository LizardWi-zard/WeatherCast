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

            CheckDirectoryExistion();
        }

        public delegate void OnWeatherUpdated(object? sender, WeatherUpdatedEventArgs? e);

        public event OnWeatherUpdated OnWeatherAutoUpdate;

        public CurrentWeather GetCurrentWeather(string cityName)
        {
            Validate.CityName(cityName, nameof(cityName));

            CurrentWeather weather;
            DateTime lastRequestTime = DateTime.Now;
            SettingsProvider settings = SettingsProvider.getInstance();

            string fileCityName;

            LastRequestCurrentInfo lastRequestInfo;

            lastRequestInfo = (LastRequestCurrentInfo)ReadFromFileOfCertainInfoType(typeof(LastRequestCurrentInfo).ToString());

            if (lastRequestInfo != null)
            {
                lastRequestTime = lastRequestInfo.RequestTime;

                fileCityName = lastRequestInfo.CityName.ToLower();

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

                    lastRequestInfo = new LastRequestCurrentInfo()
                    {
                        CityName = cityName,
                        RequestTime = DateTime.Now
                    };
                    
                    SaveLastCurrentRequestInfo(lastRequestInfo);
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

        public ForecastWeather GetForecastWeather(string longitude, string latitude, bool isPlannedRequest)
        {
            Validate.GeographicCoordinateValue(longitude, "longitude");
            Validate.GeographicCoordinateValue(latitude, "latitude");

            ForecastWeather weather;
            DateTime lastRequestTime = DateTime.Now;
            SettingsProvider settings = SettingsProvider.getInstance();

            LastRequestForecastInfo lastRequestInfo;

            lastRequestInfo = (LastRequestForecastInfo)ReadFromFileOfCertainInfoType(typeof(LastRequestForecastInfo).ToString());

            if (lastRequestInfo != null)
            {
                lastRequestTime = lastRequestInfo.RequestTime;
            }
            else
            {
                lastRequestTime = settings.LastRequestTime;
            }

            var difference = DateTime.Now.Subtract(lastRequestTime);
            if (difference >= upadteCacheInterval || isPlannedRequest)
            {
                try
                {
                    weather = internetDataProvider.GetForecastWeather(longitude, latitude, true);

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
                    weather = fileDataProvider.GetForecastWeather(longitude, latitude, true);
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
            List<CurrentWeather> markedCitiesWeather;

            markedCitiesWeather = (List<CurrentWeather>)ReadFromFileOfCertainInfoType(typeof(List<CurrentWeather>).ToString());

            if (markedCitiesWeather != null)
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
            LastRequestCurrentInfo lastRequestInfo;

            lastRequestInfo = (LastRequestCurrentInfo)ReadFromFileOfCertainInfoType(typeof(LastRequestCurrentInfo).ToString());

            var current = GetCurrentWeather(lastRequestInfo.CityName);

            var longitude = current.Coord.Longitude.ToString();
            var latitude = current.Coord.Latitude.ToString();

            var forecast = GetForecastWeather(longitude, latitude, true);

            var args = new WeatherUpdatedEventArgs(current, forecast);

            OnWeatherAutoUpdate?.Invoke(this, args);
        }

        private object ReadFromFileOfCertainInfoType(string fileInfoType)
        {
            string filePath;

            var FileType = typeof(LastRequestCurrentInfo);

            switch (fileInfoType)
            {
                case "WeatherCast.DataProvider.LastRequestCurrentInfo":
                    filePath = Definitions.RequestCurrentInfoTimePath;
                    LastRequestCurrentInfo lastRequestCurrentInfo;
                    if (File.Exists(filePath))
                    {
                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            JsonTextReader reader = new JsonTextReader(sr);
                            lastRequestCurrentInfo = new JsonSerializer().Deserialize<LastRequestCurrentInfo>(reader);
                        }
                    }
                    else
                    {
                        lastRequestCurrentInfo = null;
                    }
                    return lastRequestCurrentInfo;

                case "WeatherCast.DataProvider.LastRequestForecastInfo":
                    filePath = Definitions.RequestFutureInfoTimePath;
                    LastRequestForecastInfo lastRequesForecasttInfo;
                    if (File.Exists(filePath))
                    {
                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            JsonTextReader reader = new JsonTextReader(sr);
                            lastRequesForecasttInfo = new JsonSerializer().Deserialize<LastRequestForecastInfo>(reader);
                        }
                    }
                    else
                    {
                        lastRequesForecasttInfo = new LastRequestForecastInfo();
                    }
                    return lastRequesForecasttInfo;


                case "System.Collections.Generic.List`1[WeatherCast.Model.CurrentWeather]":
                    filePath = Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath;
                    List<CurrentWeather> lastRequestListInfo;
                    if (File.Exists(filePath))
                    {
                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            JsonTextReader reader = new JsonTextReader(sr);
                            lastRequestListInfo = new JsonSerializer().Deserialize<List<CurrentWeather>>(reader);
                        }
                    }
                    else
                    {
                        lastRequestListInfo = new List<CurrentWeather>();
                    }
                    return lastRequestListInfo;


                default:
                    return null;
            }
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

        private void CheckDirectoryExistion()
        {
            if (File.Exists(Definitions.DirectoryPath)) //TODO: find why something creating directory as file
            {
                File.Delete(Definitions.DirectoryPath);
            }

            if (!Directory.Exists(Definitions.DirectoryPath))
            {
                Directory.CreateDirectory(Definitions.DirectoryPath);
            }
        }
    }
}
