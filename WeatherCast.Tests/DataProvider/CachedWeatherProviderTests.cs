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
        protected override IDataProvider CreateTestTarget()
        {
            return new CachedWeatherProvider(new MockDataProvider(), new MockDataProvider(), TimeSpan.FromSeconds(2));
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

            // action
            var result = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsFalse(internetProviderMock.GetCurrentWeatherIsCalled);
            Assert.IsTrue(fileProviderMock.GetCurrentWeatherIsCalled);
        }

        [Test]
        public void GetCurrentWeather_InternetServiceCallIfTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock, TimeSpan.Zero);

            // action
            var result = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsTrue(internetProviderMock.GetCurrentWeatherIsCalled);
            Assert.IsFalse(fileProviderMock.GetCurrentWeatherIsCalled);
            Assert.That(CurrentWeather.Empty, Is.SameAs(result));
        }
    }
}
