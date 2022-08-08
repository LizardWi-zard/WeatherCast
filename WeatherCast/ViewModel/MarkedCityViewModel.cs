using System;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class MarkedCityViewModel : ViewModelBase
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
    }
}
