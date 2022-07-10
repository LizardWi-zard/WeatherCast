using System.IO;

namespace WeatherCast
{
    internal static class Definitions
    {
        internal static readonly string RequestTimePath = Path.Combine(Directory.GetCurrentDirectory(), "requestTime.txt");

        internal static readonly string SelectedCityInfoPath = Path.Combine(Directory.GetCurrentDirectory(), "SelectedCityInfo.txt");
    }
}
