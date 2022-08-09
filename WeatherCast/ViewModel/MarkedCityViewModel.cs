using Newtonsoft.Json;
using System;
using System.IO;
using WeatherCast.Core;
using WeatherCast.DataProvider;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    internal class MarkedCityViewModel : ViewModelBase
    {

        public delegate void OnButtonClick(object? sender, OnButtonClick? e);

        public event OnButtonClick OnButtonClickEvent;

        public MarkedCityViewModel(CurrentWeather weather, CachedWeatherProvider provider)
        {
            this.CurrentWeather = weather;

            ChangeViewCommand = new RelayCommand(o =>
            {
                OnButtonClickEvent?.Invoke(this, null);
            });

            //MarkedCities = new CurrentWeather[] {weather, weather1, weather2};

            GetMarkedCities();

            //GetMarkedCities();
        }

        public CurrentWeather _currentWeather { get; set; }

        public RelayCommand ChangeViewCommand { get; set; }

        public CurrentWeather[] MarkedCities { get; set; }

        public CurrentWeather CurrentWeather
        {
            get { return _currentWeather; }
            set
            {
                _currentWeather = value;

                RaisePropertyChanged(nameof(CurrentWeather));
            }
        }

        private void GetMarkedCities()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "MarkedCitiesCurrentInfo.txt")))
            {
                JsonTextReader reader = new JsonTextReader(sr);
                MarkedCities = new JsonSerializer().Deserialize<CurrentWeather[]>(reader);
            }
        }

        private void WriteMarkedCities()
        {
            using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "MarkedCitiesCurrentInfo.txt")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, MarkedCities);
                sw.Close();
            }
        }
    }
}
