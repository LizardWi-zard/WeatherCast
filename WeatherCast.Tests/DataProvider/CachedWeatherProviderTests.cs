using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCast.DataProvider;
using WeatherCast.Tests.Mocks;

namespace WeatherCast.Tests.DataProvider
{
    internal class CachedWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTestTarget()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetCurrentWeather_CacheCallIfNotTimedOutTest()
        {
            // setup
            var internetProviderMock = new MockDataProvider();
            var fileProviderMock = new MockDataProvider();
            var target = new CachedWeatherProvider(internetProviderMock, fileProviderMock);

            // action
            var result = target.GetCurrentWeather("Москва");

            // assertion
            Assert.IsFalse(internetProviderMock.GetCurrentWeatherIsCalled);
            Assert.IsTrue(fileProviderMock.GetCurrentWeatherIsCalled);
        }
    }
}
