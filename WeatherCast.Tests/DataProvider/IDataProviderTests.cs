using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    public abstract class IDataProviderTests
    {
        protected abstract IDataProvider CreateTarget();

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("        ")]
        [TestCase("💩")]
        public void GetCurrentWeather_InvalidArgumentTest(string invalidArgument)
        {
            var target = CreateTarget();

            Assert.Throws<ArgumentException>(() => target.GetCurrentWeather(invalidArgument));
        }

        // example of correctly latitude and longitude: 54.196291,37.621648
        [TestCase(null, "37.621648")]
        [TestCase("", "37.621648")]
        [TestCase(" ", "37.621648")]
        [TestCase("        ", "37.621648")]
        [TestCase("-1", "37.621648")]
        [TestCase("abc", "37.621648")]
        [TestCase("💩", "37.621648")]
        public void GetForecastWeather_FirstArgumentIsInvalidTest(string invalidLongitude, string invalidLatitude)
        {
            var target = CreateTarget();

            Assert.Throws<ArgumentException>(() => target.GetForecastWeather(invalidLongitude, invalidLatitude));
        }

        // example of correctly latitude and longitude: 54.196291,37.621648
        [TestCase("54.196291", null)]
        [TestCase("54.196291", "")]
        [TestCase("54.196291", " ")]
        [TestCase("54.196291", "       ")]
        [TestCase("54.196291", "-1")]
        [TestCase("54.196291", "abc")]
        [TestCase("54.196291", "💩")]
        public void GetForecastWeather_SecondArgumentIsInvalidTest(string invalidLongitude, string invalidLatitude)
        {
            var target = CreateTarget();

            Assert.Throws<ArgumentException>(() => target.GetForecastWeather(invalidLongitude, invalidLatitude));
        }
    }
}