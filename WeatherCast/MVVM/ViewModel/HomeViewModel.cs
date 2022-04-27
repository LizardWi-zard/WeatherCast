﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.MVVM.ViewModel
{
    public class HomeViewModel
    {
        public CurrentWeather Response { get; set; }

        APIControl Control { get; set; }

        public CurrentWeather CurrentWeather { get; set; }

        public ForecastWeather ForecastWeather { get; set; }

        public string Title { get; set; }

        public string WelcomeText { get; set; }

        public HomeViewModel(APIControl control, CurrentWeather weather)
        {
            this.Control = control;
            CurrentWeather = weather;

            control.CreateFutureWeatherUrl(weather.Coord.Lon.ToString(), weather.Coord.Lat.ToString());

            ForecastWeather = control.FutureWeather();
            foreach (var day in ForecastWeather.Daily)
            {
                day.Date = ForecastWeather.GetDate(day.Dt);
                day.Temp.Day = ForecastWeather.TempToInt(day.Temp.Day);
                day.Temp.Night = ForecastWeather.TempToInt(day.Temp.Night);
            }


            ForecastWeather.ForecastFor24Hours = new List<HourCast>();

            for(int i = 0; i < 24; i++)
            {
                ForecastWeather.ForecastFor24Hours.Add(ForecastWeather.Hourly[i]);
            }

            foreach (var hour in ForecastWeather.ForecastFor24Hours)
            {
                hour.Date = ForecastWeather.GetDate(hour.Dt);
                hour.Temp = ForecastWeather.TempToInt(hour.Temp);
                hour.Feels_Like = ForecastWeather.TempToInt(hour.Feels_Like);
            }

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
