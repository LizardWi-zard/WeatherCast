using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Core;
using WeatherCast.DataProvider;
using WeatherCast.Model;
using WeatherCast.Helpers;

namespace WeatherCast.ViewModel
{
    internal class MarkedCityViewModel : ViewModelBase
    {

        public delegate void OnButtonClick(object? sender, OnButtonClick? e);

        public event OnButtonClick OnButtonClickEvent;

        public MarkedCityViewModel(CurrentWeather weather, CachedWeatherProvider provider)
        {
            this.provider = provider;
            this.CurrentWeather = weather;

            ChangeViewCommand = new RelayCommand(o =>
            {
                OnButtonClickEvent?.Invoke(this, null);
            });

            MarkedCities = provider.GetMarkedCityCurrentWeather(MarkedCitiesNames);
        }

        public CachedWeatherProvider provider;

        private CurrentWeather _currentWeather;

        private CurrentWeather[] _markedCities;

        public RelayCommand ChangeViewCommand { get; set; }

        public List<string> MarkedCitiesNames { get; set; } = new List<string>(); 

        public CurrentWeather[] MarkedCities
        {
            get { return _markedCities; }
            set
            {
                _markedCities = value;

                RaisePropertyChanged(nameof(MarkedCities));
            }
        }

        public CurrentWeather CurrentWeather
        {
            get { return _currentWeather; }
            set
            {
                _currentWeather = value;

                RaisePropertyChanged(nameof(CurrentWeather));
            }
        }

        public void AddCity(string name)
        {
            Validate.CityName(name, nameof(name));
        
            if( MarkedCitiesNames.Count() <= 5)
            {
                MarkedCitiesNames.Add(name);
            }
            else
            {
                MarkedCitiesNames.RemoveAt(0);
                MarkedCitiesNames.Add(name);
            }
        }
        
        public void UpdateMarkedCitiesInfo()
        {
            List<CurrentWeather> weathers = new List<CurrentWeather>();
        
            foreach(var item in MarkedCitiesNames)
            {
                weathers.Add(provider.GetCurrentWeather(item.ToString()));
            }
        
            MarkedCities = weathers.ToArray();
        }

        private void GetMarkedCities()
        {
            using (StreamReader sr = new StreamReader(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                JsonTextReader reader = new JsonTextReader(sr);
                MarkedCities = new JsonSerializer().Deserialize<CurrentWeather[]>(reader);
            }
        }

        private void WriteMarkedCities()
        {
            using (StreamWriter sw = File.CreateText(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, MarkedCities);
                sw.Close();
            }
        }
    }
}
