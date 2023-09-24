using System.Drawing;

namespace PixelSnapper.WPF.Converters
{
    public class GenericRgb : BaseColorConverter
    {
        public override string Title => "Generic Rgb";

        public override string Convert(System.Drawing.Color color)
        {
            return $"rgb({color.R}, {color.G}, {color.B})";
        }
    }
}
