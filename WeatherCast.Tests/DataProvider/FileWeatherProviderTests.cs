using WeatherCast.DataProvider;

namespace WeatherCast.Tests.DataProvider
{
    internal class FileWeatherProviderTests : IDataProviderTests
    {
        protected override IDataProvider CreateTarget()
        {
            return new FileWeatherProvider();
        }
    }
}
