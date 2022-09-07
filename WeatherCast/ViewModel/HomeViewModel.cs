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
        private string _backgroundImage;
        private DailyCast _selectedItem;
        private CurrentWeather _currentWeather;
        private ForecastWeather _forecastWeather;
        private Timer timer = new Timer();
        private CachedWeatherProvider _cachedWeatherProvider;

        public HomeViewModel(CachedWeatherProvider cachedWeatherProvider)
        {
            _cachedWeatherProvider = cachedWeatherProvider; 

            CurrentWeather = _cachedWeatherProvider.GetCurrentWeather(Definitions.DefaultCity);

            latitude = CurrentWeather.Coord.Latitude.ToString();
            longitude = CurrentWeather.Coord.Longitude.ToString();

            ForecastWeather = _cachedWeatherProvider.GetForecastWeather(longitude, latitude);

            WelcomeText = SetMessageByTime();

            BackgroundImage = SetBackgroundImage(CurrentWeather);
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

                ResetSelectedItem();

                RaisePropertyChanged(nameof(ForecastWeather));
            }
        }

        public string BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                _backgroundImage = value;

                RaisePropertyChanged(nameof(BackgroundImage));
            }
        }

        public string Title { get; set; }

        public string WelcomeText { get; set; }

        public int SelectedItemIndex { get; set; } = 0;

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

        public void UpdateData(CurrentWeather currentWeather)
        {
            CurrentWeather = currentWeather;

            latitude = CurrentWeather.Coord.Latitude.ToString();
            longitude = CurrentWeather.Coord.Longitude.ToString();

            ForecastWeather = _cachedWeatherProvider.GetForecastWeather(longitude, latitude);

            BackgroundImage = SetBackgroundImage(CurrentWeather);
        }

        public void UpdateData(CurrentWeather currentWeather, ForecastWeather forecastWeather)
        {
            CurrentWeather = currentWeather;
            ForecastWeather = forecastWeather;

            BackgroundImage = SetBackgroundImage(CurrentWeather);
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

        static string SetBackgroundImage(CurrentWeather CurrentWeather)
        {
            string link = @"..\Images\Background\";

            string currentWeather = CurrentWeather.Weather[0].Main.ToLower();

            TimeSpan time = DateTime.Now.TimeOfDay;

            if (new TimeSpan(21, 0, 0) < time || new TimeSpan(4, 0, 0) >= time)
            {
                return link + "Night.png";
            }

            switch (currentWeather)
            {                
                case "clouds":
                    return link + "Clouds.png";

                case "clear":
                    return link + "Clear.png";

                case "drizzle":
                    return link + "Drizzle.png";

                case "rain":
                    return link + "Rain.png";

                case "snow":
                    return link + "Snow.png";

                case "thunderstorm":
                    return link + "Thunderstorm.png";

                default:
                    return link + "Clear.png";
            }
        }

        private void ResetSelectedItem()
        {
            SelectedItemIndex = 0;

            SelectedItem = ForecastWeather.Daily[0];
        }
    }
}