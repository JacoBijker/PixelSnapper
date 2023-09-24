using System.Drawing;

namespace PixelSnapper.WPF.Models
{
    public class HslColor
    {
        public float H { get; set; }
        public float S { get; set; }
        public float L { get; set; }

        public HslColor(System.Drawing.Color color)
        {
            H = color.GetHue();
            S = color.GetSaturation();
            L = color.GetBrightness();
        }
    }
}
