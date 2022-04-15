using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.MVVM.ViewModel
{
    class SearchViewModel
    {
        public APIControl Control {  get; set; }

        public WeatherResponse Response { get; set; }

        public string Title { get; set; }

        public void UpdateControlResponse(APIControl Control, WeatherResponse Response)
        {
            this.Control = Control;
            this.Response = Response;
        }
    }
}
