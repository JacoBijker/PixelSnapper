using System.Drawing;

namespace PixelSnapper.WPF.Models
{
    public class HexColor
    {
        public string R { get; set; }
        public string G { get; set; }
        public string B { get; set; }

        public HexColor(System.Drawing.Color color)
        {
            R = color.R.ToString("X2");
            G = color.G.ToString("X2");
            B = color.B.ToString("X2");
        }
    }
}
