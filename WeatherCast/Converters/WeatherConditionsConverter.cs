using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class WeatherConditionsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var condition = value.ToString().ToLower();

            switch (condition)
            {
                case "thunderstorm":
                    return "Гроза";

                case "drizzle":
                    return "Морось";

                case "rain":
                    return "Дождь";

                case "snow":
                    return "Снег";

                case "clear":
                    return "Ясно";

                case "clouds":
                    return "Облачно";

                default:
                    return "Не найдено";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float temperature = (float)value;
            return temperature;
        }
    }
}
