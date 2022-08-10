using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal const string DefaultCity = "Москва";

        internal const string DefaultLongitude = "37,618423";

        internal const string DefaultLatitude = "55,751244";

        internal static readonly string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData");

        internal static readonly string RequestCurrentInfoTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestCurrentInfoTime.txt");

        internal static readonly string RequestFutureInfoTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestFutureInfoTime.txt");

        internal static readonly string SelectedCityCurrenWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityCurrentInfo.txt");

        internal static readonly string SelectedCityFutureWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityFutureInfo.txt");

        internal static readonly string MarkedCitiesCurrenWeatherWeatherInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "MarkedCitiesCurrentInfo.txt");
    }
}
