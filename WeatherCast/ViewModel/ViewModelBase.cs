using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        
        private DateTime lastRequestTime;
        protected CurrentWeather updatedInfo;
        protected WeatherService control;
        public string selectedCity;

        public ViewModelBase(WeatherService control)
        {
            this.control = control;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
