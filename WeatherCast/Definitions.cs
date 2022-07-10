using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal static readonly string RequestTimePath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "RequestTime.txt");

        internal static readonly string SelectedCityInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SavedData", "SelectedCityInfo.txt");
    }
}
