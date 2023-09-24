using PixelSnapper.Magnification;
using PixelSnapper.WPF.Converters;
using PixelSnapper.WPF.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PixelSnapper.WPF.Windows
{
    public partial class Lens : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );

        private bool cursorHidden = false;
        private FormMagnifier magnifier;
        private Bullseye bullseye;

        public Lens()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, Width, Height));

            bullseye = new Bullseye();
            
            magnifier = new FormMagnifier(this, bullseye);

            this.MouseWheel += Lens_MouseWheel;
            this.FormClosing += Lens_FormClosing;
            this.MouseClick += Lens_MouseClick;
        }

        private void Lens_MouseClick(object sender, MouseEventArgs e)
        {
            Hide(true);

            POINT mousePoint = new POINT();
            NativeMethods.GetCursorPos(ref mousePoint);

            ColorHolder.CurrentColor = Color.ColorGrabber.GetColorAt(mousePoint.x, mousePoint.y);

            var converter = ServiceLocator.Resolve<BaseColorConverter>();
            var toClipboard = converter.Convert(ColorHolder.CurrentColor);

            Clipboard.SetText(toClipboard);
            bullseye.SnapColor();

        }

        public new void Show()
        {
            magnifier.Show();
            bullseye.Show();
            base.Show();

            if (!cursorHidden)
            {
                cursorHidden = true;
                Cursor.Hide();
            }
        }

        public new void Hide()
        {
            Hide(false);
        }

        public void Hide(bool snapColor)
        {
            base.Hide();
            if (!snapColor)
                bullseye.Hide();

            magnifier.Hide();

            if (cursorHidden)
            {
                cursorHidden = false;
                Cursor.Show();
            }
        }

        private void Lens_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                magnifier.Magnification = Math.Min(magnifier.Magnification + 1f, 10);
            else
                magnifier.Magnification = Math.Max(magnifier.Magnification - 1f, 2);
        }

        private void Lens_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= Lens_FormClosing;
            this.MouseWheel -= Lens_MouseWheel;
        }
    }
}
