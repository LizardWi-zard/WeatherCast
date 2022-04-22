﻿using System;
using System.Collections.Generic;
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

        public string Title { get; set; }

        public string WelcomeText { get; set; }


        public HomeViewModel(APIControl control, CurrentWeather weather)
        {
            this.Control = control;
            CurrentWeather = weather;

            //Response = control.GetResponse();

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
