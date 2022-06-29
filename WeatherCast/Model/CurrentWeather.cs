namespace WeatherCast.Model
{
    public class CurrentWeather
    {
        public string Name { get; set; }

        public Coordinates Coord { get; set; }

        public CurrentWeatherInfo[] Weather { get; set; }

        public CurrentTempInfo Main { get; set; }

        public WindInfo Wind { get; set; }
    }
}
