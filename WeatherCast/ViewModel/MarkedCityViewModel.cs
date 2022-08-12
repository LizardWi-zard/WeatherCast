using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Core;
using WeatherCast.DataProvider;
using WeatherCast.Model;
using WeatherCast.Helpers;
using System.Windows.Input;

namespace WeatherCast.ViewModel
{
    internal class MarkedCityViewModel : ViewModelBase
    {
        private SettingsProvider singleton = SettingsProvider.getInstance();
        private CurrentWeather _currentWeather;
        private List<CurrentWeather> _markedCities;

        public delegate void OnButtonClick(object? sender, OnButtonClick? e);

        public event OnButtonClick OnButtonClickEvent;

        public MarkedCityViewModel(CurrentWeather weather, CachedWeatherProvider provider)
        {
            this.provider = provider;
            this.CurrentWeather = weather;

            AddCityCommand = new RelayCommand(o =>
            {
                MarkedCities = provider.GetMarkedCitiesCurrentWeather(singleton.CitiyNames);

                UpdateMarkedCitiesNames();
            });

            RemoveCityCommand = new RelayCommand(o =>
            {
                singleton = SettingsProvider.getInstance();

                singleton.CitiyNames.RemoveAt(SelectedMarkedCity);
                MarkedCities.RemoveAt(SelectedMarkedCity);

                MarkedCities = provider.GetMarkedCitiesCurrentWeather(singleton.CitiyNames);
            });

            ChangeViewCommand = new RelayCommand(o =>
            {
                OnButtonClickEvent?.Invoke(this, null);
            });

            MarkedCities = provider.GetMarkedCitiesCurrentWeather(singleton.CitiyNames);

            UpdateMarkedCitiesNames();
        }


        public CachedWeatherProvider provider;


        public RelayCommand AddCityCommand { get; set; }

        public RelayCommand RemoveCityCommand { get; set; }

        public RelayCommand ChangeViewCommand { get; set; }

        public int SelectedMarkedCity { get; set; }

        public List<string> MarkedCitiesNames { get; set; } = new List<string>(); 

        public List<CurrentWeather> MarkedCities
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
        
        public void UpdateMarkedCitiesNames()
        {
            singleton = SettingsProvider.getInstance();

            singleton.CitiyNames.Clear();

            foreach (var item in MarkedCities)
            {
                singleton.CitiyNames.Add(item.Name);
            }
        }

        private void GetMarkedCities()
        {
            using (StreamReader sr = new StreamReader(Definitions.MarkedCitiesCurrenWeatherWeatherInfoPath))
            {
                JsonTextReader reader = new JsonTextReader(sr);
                MarkedCities = new JsonSerializer().Deserialize<List<CurrentWeather>>(reader);
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
