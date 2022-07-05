using System;
using System.Timers;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private string longitude;
        private string latitude;
        private DailyCast _selectedItem;
        private string path = @"D:\WeatherCast\requestTime.txt";
        private Timer timer = new Timer();
        //private TimerEventManager _timerEventManager;

        public HomeViewModel(WeatherService control, CurrentWeather weather) :
            base(control, weather)
        {
            this.control = control;
            CurrentWeather = weather;

            latitude = CurrentWeather.Coord.Latitude.ToString();
            longitude = CurrentWeather.Coord.Longitude.ToString();

            ForecastWeather = control.GetForecastWeather(longitude, latitude);

            WelcomeText = SetMessageByTime();

            BackgroundImg = SetBackgroundImg(CurrentWeather);

            timer.Interval = 1000 * 60 * 2;

            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        public CurrentWeather CurrentWeather { get; set; }

        public ForecastWeather ForecastWeather { get; set; }

        public string Title { get; set; }

        public string BackgroundImg { get; set; }

        public string WelcomeText { get; set; }

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

            if (currentWeather != "rain" && currentWeather != "clouds")
            {
                return link + "clouds.png";
            }
            else
            {
                return link + currentWeather + ".png";
            }
        }

        private void OnTimedEvent(object sourse, ElapsedEventArgs e)
        {
            CurrentWeather = updatedInfo;
            
            RaisePropertyChanged(nameof(updatedInfo));
        }

    }
}