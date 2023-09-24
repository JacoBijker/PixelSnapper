using System.Drawing;
using PixelSnapper.WPF.Models;
using System;

namespace PixelSnapper.WPF.Converters
{
    public class GenericHsl : BaseColorConverter
    {
        public override string Title => "Generic Hsl";

        public override string Convert(System.Drawing.Color color)
        {
            var col = new HslColor(color);

            return $"hsl({Math.Round(col.H, 3)}, {Math.Round(col.S, 2) * 100}%, {Math.Round(col.L, 2) * 100}%)";
        }
    }
}
