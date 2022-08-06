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

namespace WeatherCast.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private CurrentWeather currentWeather;
        private object _currentView;
        private DateTime lastRequestTime;
        private string selectedCity = "Москва";
        private Timer timer = new Timer();
        private WeatherService control;
        private CachedWeatherProvider cachedWeatherProvider;

        public MainViewModel()
        {
            cachedWeatherProvider = new CachedWeatherProvider(new InternetWeatherProvider(), new FileWeatherProvider(), TimeSpan.FromMinutes(15));

            control = new WeatherService();
            
            VMBase = new ViewModelBase();

            HomeVM = new HomeViewModel(cachedWeatherProvider);
            SettingsVM = new SettingsViewModel(HomeVM.CurrentWeather.Name);

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            cachedWeatherProvider.OnWeatherAutoUpdate += UpdateData;

            SearchVM = new SearchViewModel();

            
            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });
            
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

        public SearchViewModel SearchVM { get; set; }

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

        private void UpdateData(object sourse, WeatherUpdatedEventArgs? e)
        {
            HomeVM.CurrentWeather = e.CurrentWeather;
            HomeVM.ForecastWeather = e.ForecastWeather;
        }
    }
}
