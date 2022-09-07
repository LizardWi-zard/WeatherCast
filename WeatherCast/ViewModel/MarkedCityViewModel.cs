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
        private CurrentWeather _selectedMarkedCity;
        private CachedWeatherProvider provider;

        public delegate void OnCityChangedEvent(object? sender, CurrentCityCangedArgs? e);

        public event OnCityChangedEvent ChangeViewEvent;

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
                var args = new CurrentCityCangedArgs(weather);

                ChangeViewEvent?.Invoke(this, args);
            });

            MarkedCities = provider.GetMarkedCitiesCurrentWeather(singleton.CitiyNames);

            UpdateMarkedCitiesNames();
        }

        public RelayCommand AddCityCommand { get; set; }

        public RelayCommand RemoveCityCommand { get; set; }

        public RelayCommand ChangeViewCommand { get; set; }

        public int SelectedMarkedCity { get; set; }

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

        public CurrentWeather SelectedCurrentWeather
        {
            get { return _selectedMarkedCity; }
            set
            {
                _selectedMarkedCity = value;

                var args = new CurrentCityCangedArgs(_selectedMarkedCity);

                ChangeViewEvent?.Invoke(this, args);

                RaisePropertyChanged(nameof(SelectedCurrentWeather));
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
    }
}
