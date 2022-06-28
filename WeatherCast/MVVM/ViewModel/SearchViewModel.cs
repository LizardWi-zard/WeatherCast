using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.MVVM.ViewModel
{
    public class SearchViewModel
    {
        public WeatherService Control { get; set; }

        public CurrentWeather Response { get; set; }

        public string Title { get; set; }

        public SearchViewModel()
        {
           // _2timerEventManager = new TimerEventManager(@"D:\WeatherCast\seare.txt");
        }

        public void UpdateControlResponse(WeatherService Control, CurrentWeather Response)
        {
            this.Control = Control;
            this.Response = Response;
        }
    }
}
