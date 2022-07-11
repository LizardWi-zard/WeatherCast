using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    internal class InternetWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTarget()
        {
            return new InternetWeatherProvider();
        }
    }
}
