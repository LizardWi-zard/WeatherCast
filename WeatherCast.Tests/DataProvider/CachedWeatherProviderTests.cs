using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromSeconds(5));
            CurrentWeather result = new CurrentWeather(); 

            // action
            result = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsFalse(internetProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsTrue(fileProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsInstanceOf(typeof(CurrentWeather), result);
            Assert.That(CurrentWeather.Empty, Is.SameAs(result));
            
        }

        [Test]
        public void GetCurrentWeather_InternetServiceCallIfTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.Zero);
            CurrentWeather result = new CurrentWeather();

            // action
            result = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsTrue(internetProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsFalse(fileProviderMock.GetCurrentWeatherWasCalled);
            Assert.IsInstanceOf(typeof(CurrentWeather), result);
            Assert.That(CurrentWeather.Empty, Is.SameAs(result));
        }

        [Test]
        public void GetForecastWeather_CacheCallIfNotTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.FromSeconds(5));
            ForecastWeather result = new ForecastWeather();

            // action
            result = target.GetForecastWeather("54,196291", "37,621648");

            // assertion
            Assert.IsFalse(internetProviderMock.GetForecastWeatherWasCalled);
            Assert.IsTrue(fileProviderMock.GetForecastWeatherWasCalled);
            Assert.IsInstanceOf(typeof(ForecastWeather), result);
            Assert.That(ForecastWeather.Empty, Is.SameAs(result));
        }

        [Test]
        public void GetForecastWeather_InternetServiceCallIfTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.Zero);
            ForecastWeather result = new ForecastWeather();

            // action
            result = target.GetForecastWeather("54,196291", "37,621648");

            // assertion
            Assert.IsTrue(internetProviderMock.GetForecastWeatherWasCalled);
            Assert.IsFalse(fileProviderMock.GetForecastWeatherWasCalled);
            Assert.IsInstanceOf(typeof(ForecastWeather), result);
            Assert.That(ForecastWeather.Empty, Is.SameAs(result));
        }
    }
}
