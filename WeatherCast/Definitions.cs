using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal static readonly string RequestTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestTime.txt");

        internal static readonly string SelectedCityCurrentInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityCurrentInfo.txt");

        internal static readonly string SelectedCityFutureInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityFutureInfo.txt");
    }
}
