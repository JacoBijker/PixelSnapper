using System.Drawing;

namespace PixelSnapper.WPF.Converters
{
    public class GenericRgbA : BaseColorConverter
    {
        public override string Title => "Generic RgbA";

        public override string Convert(System.Drawing.Color color)
        {
            return $"rgba({color.R}, {color.G}, {color.B}, 255)";
        }
    }
}
