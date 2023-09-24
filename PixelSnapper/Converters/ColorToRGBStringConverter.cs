using System;
using System.Globalization;
using System.Windows.Data;

namespace PixelSnapper.Converters
{
    public class ColorToRGBStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = (System.Drawing.Color)value;

            if (col == null)
                return "No color";

            return $"({col.R}, {col.G}, {col.B})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
