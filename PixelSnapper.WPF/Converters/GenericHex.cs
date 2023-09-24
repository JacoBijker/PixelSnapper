using System.Drawing;
using PixelSnapper.WPF.Models;

namespace PixelSnapper.WPF.Converters
{
    public class GenericHex : BaseColorConverter
    {
        public override string Title => "Generic Hex";

        public override string Convert(System.Drawing.Color color)
        {
            HexColor col = new HexColor(color);
            return $"#{col.R}{col.G}{col.B}";
        }
    }
}
