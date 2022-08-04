using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class SearchViewModel
    {
        public WeatherService Control { get; set; }

        public CurrentWeather Response { get; set; }

        public string Title { get; set; }

        public void UpdateControlResponse(WeatherService Control, CurrentWeather Response)
        {
            this.Control = Control;
            this.Response = Response;
        }
    }
}
