namespace WeatherCast.Model
{
    public class CurrentWeather
    {
        public string Name { get; set; }

        public Coordinates Coord { get; set; }

        public CurrentWeatherInfo[] Weather { get; set; }

        public CurrentTempInfo Main { get; set; }

        public WindInfo Wind { get; set; }

        public static CurrentWeather Empty { get; } = new CurrentWeather()
        {
            Coord = new Coordinates(),
            Weather = new CurrentWeatherInfo[0],
            Main = new CurrentTempInfo(),
            Wind = new WindInfo(),
        };
    }
}
