using System;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class MarkedCityViewModel
    {
        public delegate void OnButtonClick(object? sender, OnButtonClick? e);

        public event OnButtonClick OnButtonClickEvent;

        public MarkedCityViewModel(CurrentWeather weather)
        {
            this.CurrentWeather = weather;

            ChangeViewCommand = new RelayCommand(o =>
            {
                OnButtonClickEvent?.Invoke(this, null);
            });

            MarkedCities = new CurrentWeather[] {CurrentWeather.Empty, CurrentWeather.Empty, CurrentWeather.Empty, CurrentWeather.Empty };
        }

        public CurrentWeather CurrentWeather { get; set; }

        public RelayCommand ChangeViewCommand { get; set; }

        public CurrentWeather[] MarkedCities { get; set; }
    }
}
