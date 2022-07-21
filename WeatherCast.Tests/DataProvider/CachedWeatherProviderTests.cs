using WeatherCast.DataProvider;
using WeatherCast.Model;
using WeatherCast.Tests.Mocks;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    internal class CachedWeatherProviderTests : IDataProviderTests
    {
        private readonly string fileCacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SavedData");

        protected override IDataProvider CreateTestTarget()
        {
            return new CachedWeatherProvider(new MockDataProvider(), new MockDataProvider(), TimeSpan.FromSeconds(2));
        }

        [TearDown]
        public void CleanUp()
        {
            if (Directory.Exists(fileCacheDirectory))
            {
                Directory.Delete(fileCacheDirectory, recursive: true);
            }
        }

        [Test]
        public void Constructor_FirstArgumentNullTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CachedWeatherProvider(null, new MockDataProvider(), TimeSpan.Zero)
            );
        }

        [Test]
        public void Constructor_SecondArgumentNullTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CachedWeatherProvider(new MockDataProvider(), null, TimeSpan.Zero)
            );
        }

        [Test]
        public void GetCurrentWeather_CacheCallIfNotTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var expectedResult = CurrentWeather.Empty;
            fileProviderMock.ReturnForGetCurrentWeather = expectedResult;
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromSeconds(5));

            // action
            var actualResult = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsFalse(internetProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsTrue(fileProviderMock.GetCurrentWeatherWasCalled);
            Assert.AreSame(expectedResult, actualResult);

        }

        [Test]
        public void GetCurrentWeather_InternetServiceCallIfTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var expectedResult = CurrentWeather.Empty;
            internetProviderMock.ReturnForGetCurrentWeather = expectedResult;
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromTicks(1));

            // action
            var actualResult = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsTrue(internetProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsFalse(fileProviderMock.GetCurrentWeatherWasCalled);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public void GetForecastWeather_CacheCallIfNotTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var expectedResult = ForecastWeather.Empty;
            var fileProviderMock = new MockDataProvider();
            fileProviderMock.ReturnForForecastWeather = expectedResult;
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromSeconds(5));

            // action
            var actualResult = target.GetForecastWeather("54,196291", "37,621648");

            // assertion
            Assert.IsFalse(internetProviderMock.GetForecastWeatherWasCalled);
            Assert.IsTrue(fileProviderMock.GetForecastWeatherWasCalled);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public void GetForecastWeather_InternetServiceCallIfTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var expectedResult = ForecastWeather.Empty;
            internetProviderMock.ReturnForForecastWeather = expectedResult;
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromTicks(1));
            ForecastWeather result = new ForecastWeather();

            // action
            var actualResult = target.GetForecastWeather("54,196291", "37,621648");

            // assertion
            Assert.IsTrue(internetProviderMock.GetForecastWeatherWasCalled);
            Assert.IsFalse(fileProviderMock.GetForecastWeatherWasCalled);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public void GetCurrentWeather_WeatherAutoUpdateWhenTimedOut()
        {
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var timeOut = TimeSpan.FromSeconds(1);
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, timeOut);

            TestHelper.WaitAsync(timeOut).ContinueWith(_ =>
            {
                Assert.IsTrue(internetProviderMock.GetCurrentWeatherWasCalled);
                Assert.IsFalse(fileProviderMock.GetCurrentWeatherWasCalled);
            });
        }

        [Test]
        public void GetForecastWeather_WeatherAutoUpdateWhenTimedOut()
        {
            var internetProviderMock = new MockDataProvider();
            var timeOut = TimeSpan.FromSeconds(1);
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, timeOut);

            TestHelper.WaitAsync(timeOut).ContinueWith(_ =>
            {
                Assert.IsTrue(internetProviderMock.GetCurrentWeatherWasCalled);
                Assert.IsFalse(fileProviderMock.GetCurrentWeatherWasCalled);
            });
        }

        [Test]
        public void RaisePropertyChanged_WasRaised()
        {
            var internetProviderMock = new MockDataProvider();
            var timeOut = TimeSpan.FromSeconds(1);
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, timeOut);

            TestHelper.WaitAsync(timeOut).ContinueWith(_ =>
            {
                Assert.IsTrue(target.wasRaised);
            });
        }
    }
}
