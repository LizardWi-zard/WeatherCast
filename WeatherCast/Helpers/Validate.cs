using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.Helpers
{
    internal static class Validate
    {
        public static void CityName(string target, string message)
        {
            if (string.IsNullOrWhiteSpace(target) || !(target.All(c => Char.IsLetter(c) || c == '-') && target.Count(f => f == '-') < 2 && target.Length > 1))
            {
                throw new ArgumentException(message);
            }
        }

        public static void GeographicCoordinateValue(string target, string message)
        {
            if (!double.TryParse(target, out _))
            {
                throw new ArgumentException(message);
            }
        }
    }
}
