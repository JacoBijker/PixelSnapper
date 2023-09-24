using System;
using System.Globalization;
using System.Windows.Data;

namespace PixelSnapper.Converters
{
    public class ColorToHEXStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = (System.Drawing.Color)value;

            if (col == null)
                return "No color";

            return $"#FF{col.R.ToString("X2")}{col.G.ToString("X2")}{col.B.ToString("X2")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
