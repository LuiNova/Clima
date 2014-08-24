using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WeatherApp.Converters
{
    public class BoolHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength height = new GridLength(1, GridUnitType.Star);
            bool valueToConvert = (bool)value;

            if (valueToConvert)
            {
                height = new GridLength(4, GridUnitType.Star);
            }

            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
