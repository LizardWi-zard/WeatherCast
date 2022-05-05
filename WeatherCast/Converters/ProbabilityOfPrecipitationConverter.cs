using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WeatherCast.Converters
{

    [ValueConversion(typeof(DateTime), typeof(String))]
    public class ProbabilityOfPrecipitationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double inputValue = System.Convert.ToDouble(value);
            var ProbabilityOfPrecipitation = String.Format("{0:0}", inputValue * 100);
            string output = ProbabilityOfPrecipitation + "%";
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
