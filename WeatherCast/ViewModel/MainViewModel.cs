using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using WeatherCast.Core;
using WeatherCast.DataProvider;
using WeatherCast.Model;
using static WeatherCast.DataProvider.CachedWeatherProvider;
using static WeatherCast.ViewModel.MarkedCityViewModel;

namespace WeatherCast.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private CurrentWeather currentWeather;
        private object _currentView;
        private bool _isChecked = true;
        private DateTime lastRequestTime;
        private string selectedCity = "Москва";
        private Timer timer = new Timer();
        private CachedWeatherProvider cachedWeatherProvider;

        public MainViewModel()
        {
            cachedWeatherProvider = new CachedWeatherProvider(new InternetWeatherProvider(), new FileWeatherProvider(), TimeSpan.FromMinutes(1));
            
            VMBase = new ViewModelBase();

            HomeVM = new HomeViewModel(cachedWeatherProvider);
            MarkedCityVM = new MarkedCityViewModel(HomeVM.CurrentWeather, cachedWeatherProvider);
            SettingsVM = new SettingsViewModel(HomeVM.CurrentWeather.Name);

            MarkedCityVM.OnButtonClickEvent += ChangeView;

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = MarkedCityVM;
            });

            cachedWeatherProvider.OnWeatherAutoUpdate += UpdateData;

            /*
            
            SearchCommand = new RelayCommand(o =>
            {
                Response = currentWeather;
                SearchVM.UpdateControlResponse(control, Response);

                CurrentView = SearchVM;
            });
            */
        }

        public ViewModelBase VMBase { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public MarkedCityViewModel MarkedCityVM { get; set; }

        public SettingsViewModel SettingsVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand SettingsViewCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public CurrentWeather Response { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        private void UpdateData(object sourse, WeatherUpdatedEventArgs? e)
        {
            HomeVM.CurrentWeather = e.CurrentWeather;
            HomeVM.ForecastWeather = e.ForecastWeather;
            MarkedCityVM.CurrentWeather = e.CurrentWeather;
        }

        private void ChangeView(object sourse, OnButtonClick? e)
        {
            CurrentView = HomeVM;

            IsChecked = true;
        }
    }
}
