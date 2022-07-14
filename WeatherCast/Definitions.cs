using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal static readonly string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData");

        internal static readonly string RequestTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestTime.txt");

        internal static readonly string SelectedCityCurrenWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityCurrentInfo.txt");

        internal static readonly string SelectedCityFutureWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityFutureInfo.txt");
    }
}
