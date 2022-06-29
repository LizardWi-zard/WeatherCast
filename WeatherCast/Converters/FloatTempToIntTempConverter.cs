using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(float), typeof(int))]
    public class FloatTempToIntTempConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int temperature = System.Convert.ToInt32(value);
            return temperature;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float temperature = (float)value;
            return temperature;
        }
    }
}
