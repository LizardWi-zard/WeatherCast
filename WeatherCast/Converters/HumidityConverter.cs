using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class HumidityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inputValue = value;
            string output = value.ToString() + "%";
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
