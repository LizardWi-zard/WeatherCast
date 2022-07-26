using System;
using System.Timers;
using WeatherCast.DataProvider;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    internal class HomeViewModel : ViewModelBase
    {
        private string longitude;
        private string latitude;
        private DailyCast _selectedItem;
        private CurrentWeather _currentWeather;
        private ForecastWeather _forecastWeather;
        private Timer timer = new Timer();

        public HomeViewModel(CachedWeatherProvider cachedWeatherProvider)
        {
            CurrentWeather = cachedWeatherProvider.GetCurrentWeather(Definitions.DefaultCity);

            latitude = CurrentWeather.Coord.Latitude.ToString();
            longitude = CurrentWeather.Coord.Longitude.ToString();

            ForecastWeather = cachedWeatherProvider.GetForecastWeather(longitude, latitude);

            WelcomeText = SetMessageByTime();

            BackgroundImg = SetBackgroundImg(CurrentWeather);
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

        public ForecastWeather ForecastWeather
        {
            get { return _forecastWeather; }
            set
            {
                _forecastWeather = value;

                RaisePropertyChanged(nameof(ForecastWeather));
            }
        }

        public string Title { get; set; }

        public string BackgroundImg { get; set; }

        public string WelcomeText { get; set; }

        public DailyCast SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                //_selectedItem.Temperature.MaxTemperatureText = "Максимальная температура днём: " + value.Temperature.MaxTemperature.ToString(); // после обновления данных _selectedItem становиться null
                //_selectedItem.Temperature.MinTemperatureText = "Минимальная температура ночью: " + value.Temperature.MinTemperature.ToString();


                RaisePropertyChanged(nameof(SelectedItem));
            }
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

        static string SetBackgroundImg(CurrentWeather CurrentWeather)
        {
            // компьютеро-зависимый путь
            // string link = "C:/Users/ArEf/source/repos/WeatherCast/WeatherCast/Images/Background/";
            // сделать так же во всех остальных местах использования файлов
            string link = @"..\Images\Background\";

            string currentWeather = CurrentWeather.Weather[0].Main.ToLower();

            TimeSpan time = DateTime.Now.TimeOfDay;

            if (new TimeSpan(21, 0, 0) < time || new TimeSpan(4, 0, 0) >= time)
            {
                return link + "Night.png";
            }

            if (currentWeather != "rain" && currentWeather != "clouds")
            {
                return link + "clouds.png";
            }
            else
            {
                return link + currentWeather + ".png";
            }
        }


    }
}