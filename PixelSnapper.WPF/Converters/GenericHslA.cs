using PixelSnapper.WPF.Models;
using System;
using System.Drawing;

namespace PixelSnapper.WPF.Converters
{
    public class GenericHslA : BaseColorConverter
    {
        public override string Title => "Generic HslA";

        public override string Convert(System.Drawing.Color color)
        {
            var col = new HslColor(color);

            return $"hsla({Math.Round(col.H, 3)}, {Math.Round(col.S, 2) * 100}%, {Math.Round(col.L, 2) * 100}%, 1)";
        }
    }
}
