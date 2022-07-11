using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    internal class FileWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTestTarget()
        {
            return new FileWeatherProvider();
        }
    }
}
