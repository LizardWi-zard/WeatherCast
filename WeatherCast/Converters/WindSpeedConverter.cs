using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class WindSpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = value.ToString() + " м/с";
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
