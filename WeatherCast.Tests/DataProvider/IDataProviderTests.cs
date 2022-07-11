using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    public abstract class IDataProviderTests
    {
        protected abstract IDataProvider CreateTestTarget();

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("        ")]
        [TestCase("💩")]
        public void GetCurrentWeather_InvalidArgumentTest(string invalidArgument)
        {
            var target = CreateTestTarget();

            Assert.Throws<ArgumentException>(() => target.GetCurrentWeather(invalidArgument));
        }

        [Test]
        [Ignore("Not unit test (integration)")]
        public void GetCurrentWeatherTest()
        {
            var target = CreateTestTarget();

            var result = target.GetCurrentWeather("Москва");

            Assert.NotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Name));
            Assert.NotNull(result.Wind);
            CollectionAssert.IsNotEmpty(result.Weather);
            Assert.NotNull(result.Main);
            Assert.NotNull(result.Coord);
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
            var target = CreateTestTarget();

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
            var target = CreateTestTarget();

            Assert.Throws<ArgumentException>(() => target.GetForecastWeather(invalidLongitude, invalidLatitude));
        }

        [Test]
        [Ignore("Not unit test (integration)")]
        public void GetForecastWeatherTest()
        {
            var target = CreateTestTarget();

            var result = target.GetForecastWeather("54.196291", "37.621648");

            Assert.NotNull(result);
            CollectionAssert.IsNotEmpty(result.Daily);
            CollectionAssert.IsNotEmpty(result.Hourly);
            CollectionAssert.IsNotEmpty(result.ForecastFor24Hours);
            Assert.IsFalse(string.IsNullOrEmpty(result.TimeZone));
            Assert.IsTrue(result.Lat >= 0F);
            Assert.NotNull(result.Lon >= 0F);
        }
    }
}