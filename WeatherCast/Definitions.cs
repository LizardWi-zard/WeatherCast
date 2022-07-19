using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal const string DefaultCity = "Москва";

        internal const string DefaultLongitude = "37,618423";

        internal const string DefaultLatitude = "55,751244";

        internal static readonly string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData");

        internal static readonly string RequestTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestTime.txt");

        internal static readonly string SelectedCityCurrenWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityCurrentInfo.txt");

        internal static readonly string SelectedCityFutureWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityFutureInfo.txt");
    }
}
