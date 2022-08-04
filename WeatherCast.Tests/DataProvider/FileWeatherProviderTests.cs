using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    [TestFixture]
    internal class FileWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTestTarget()
        {
            return new FileWeatherProvider();
        }
    }
}
