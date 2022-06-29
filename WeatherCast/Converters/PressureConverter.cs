using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class PressureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double inputValue = System.Convert.ToDouble(value);
            var pressure = inputValue / 1.333;
            string output = string.Format("{0:0.00}", pressure) + "мм";
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double inputValue = System.Convert.ToInt32(value);
            var pressure = inputValue / 0.667;
            string output = string.Format("{0:0.00}", pressure) + "гПа";
            return output;
        }
    }
}
