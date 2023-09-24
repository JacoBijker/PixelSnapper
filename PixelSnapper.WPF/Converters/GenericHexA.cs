using PixelSnapper.WPF.Models;
using System.Drawing;

namespace PixelSnapper.WPF.Converters
{
    public class GenericHexA : BaseColorConverter
    {
        public override string Title => "Generic HexA";

        public override string Convert(System.Drawing.Color color)
        {
            HexColor col = new HexColor(color);
            return $"#FF{col.R}{col.G}{col.B}";
        }
    }
}
