using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = GetDate((int)value);
            string outputText;

            switch (parameter)
            {
                case "DayDate":
                    outputText = date.DayOfWeek.ToString() + " " + date.Day.ToString();
                    return outputText;
                
                case "HourMinutes":
                    outputText = date.ToString("HH:mm");
                    return outputText;

                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private DateTime GetDate(int unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
