using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    internal class InternetWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTestTarget()
        {
            return new InternetWeatherProvider();
        }
    }
}
