using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.MVVM.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        WeatherService Control;
        string longitude;
        string latitude;
        private DailyCast _selectedItem;

        public CurrentWeather CurrentWeather { get; set; }

        public ForecastWeather ForecastWeather { get; set; }

        public DailyCast SelectedItem
        {
            get { return _selectedItem; } 
            set
            {
                _selectedItem = value;

                _selectedItem.Temperature.MaxTemperatureText = "Максимальная температура днём: " + value.Temperature.MaxTemperature.ToString();
                _selectedItem.Temperature.MinTemperatureText = "Минимальная температура ночью: " + value.Temperature.MinTemperature.ToString();

                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public string Title { get; set; }

        public string WelcomeText { get; set; }

        public HomeViewModel(WeatherService control, CurrentWeather weather)
        {
            this.Control = control;
            CurrentWeather = weather;

            latitude = CurrentWeather.Coord.Latitude.ToString();
            longitude = CurrentWeather.Coord.Longitude.ToString();

            ForecastWeather = control.GetForecastWeather(longitude, latitude);

            WelcomeText = SetMessageByTime();
        }

        static string SetMessageByTime()
        {
            string message = null;

            TimeSpan time = DateTime.Now.TimeOfDay;

            if (new TimeSpan(0, 0, 0) < time && new TimeSpan(6, 0, 0) >= time)
            {
                message = "Доброй ночи";
            }
            else if (new TimeSpan(6, 0, 0) < time && new TimeSpan(12, 0, 0) >= time)
            {
                message = "Доброе утро";
            }
            else if (new TimeSpan(12, 0, 0) < time && new TimeSpan(18, 0, 0) >= time)
            {
                message = "Добрый день";
            }
            else if (new TimeSpan(18, 0, 0) < time && new TimeSpan(24, 0, 0) >= time)
            {
                message = "Добрый вечер";
            }

            return message;
        }
    }
}