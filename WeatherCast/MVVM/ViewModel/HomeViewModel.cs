using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.MVVM.ViewModel
{
    public class HomeViewModel
    {
        public WeatherResponse Response { get; set; }

        public string Title { get; set; }

        public HomeViewModel()
        {
            APIControl control = new APIControl();

            Response = control.GetResponse();
        }
    }
}
