using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WeatherApp.Converters
{
    public class BoolFonregroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush foreground = new SolidColorBrush(Colors.Transparent);
            bool valueToConvert = (bool)value;

            if (valueToConvert)
            {
                if (parameter != null && (string)parameter == "White")
                {
                    foreground = new SolidColorBrush(Colors.White);
                }
                else if (parameter != null && (string)parameter == "Black")
                {
                    foreground = new SolidColorBrush(Colors.Black);
                }
            }

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
