using PixelSnapper.Magnification;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelSnapper.WPF.Utilities
{
    public class FormMagnifier : IDisposable
    {
        private Form lens;
        private System.Windows.Window bullseye;
        private IntPtr hwndMag;
        private float magnification;
        private bool initialized;
        private RECT magWindowRect = new RECT();
        private Timer timer;


        public FormMagnifier(Form lens, System.Windows.Window bullseye)
        {
            this.bullseye = bullseye;

            if (lens == null)
                throw new ArgumentNullException("form");

            magnification = 2.0f;
            this.lens = lens;
            this.lens.Resize += new EventHandler(form_Resize);
            this.lens.FormClosing += new FormClosingEventHandler(form_FormClosing);

            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);

            initialized = NativeMethods.MagInitialize();
            if (initialized)
            {
                SetupMagnifier();
                timer.Interval = NativeMethods.USER_TIMER_MINIMUM;
            }
        }

        public void Show()
        {
            //Force form locations update
            POINT mousePoint = new POINT();
            NativeMethods.GetCursorPos(ref mousePoint);

            this.lens.Left = mousePoint.x - lens.Width / 2;
            this.lens.Top = mousePoint.y - lens.Height / 2;

            this.bullseye.Left = mousePoint.x - this.bullseye.Width / 2;
            this.bullseye.Top = mousePoint.y - this.bullseye.Height / 2;

            timer.Enabled = true;
        }

        public void Hide()
        {
            timer.Enabled = false;
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (bullseye != null)
                    bullseye.Close();
            }
            catch (Exception) { }

            timer.Enabled = false;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            UpdateMaginifier();
        }

        void form_Resize(object sender, EventArgs e)
        {
            ResizeMagnifier();
        }

        ~FormMagnifier()
        {
            Dispose(false);
        }

        protected virtual void ResizeMagnifier()
        {
            if (initialized && (hwndMag != IntPtr.Zero))
            {
                NativeMethods.GetClientRect(lens.Handle, ref magWindowRect);
                // Resize the control to fill the window.
                NativeMethods.SetWindowPos(hwndMag, IntPtr.Zero,
                    magWindowRect.left, magWindowRect.top, magWindowRect.right, magWindowRect.bottom, 0);
            }
        }

        public virtual void UpdateMaginifier()
        {
            if ((!initialized) || (hwndMag == IntPtr.Zero))
                return;

            POINT mousePoint = new POINT();
            RECT sourceRect = new RECT();

            NativeMethods.GetCursorPos(ref mousePoint);

            int width = (int)((magWindowRect.right - magWindowRect.left) / magnification);
            int height = (int)((magWindowRect.bottom - magWindowRect.top) / magnification);

            sourceRect.left = mousePoint.x - width / 2;
            sourceRect.top = mousePoint.y - height / 2;

            sourceRect.right = sourceRect.left + width;
            sourceRect.bottom = sourceRect.top + height;

            if (this.lens == null)
            {
                timer.Enabled = false;
                return;
            }

            if (this.lens.IsDisposed)
            {
                timer.Enabled = false;
                return;
            }

            // Set the source rectangle for the magnifier control.
            NativeMethods.MagSetWindowSource(hwndMag, sourceRect);

            // Ensure im on top of winblowz taskbar
            NativeMethods.SetWindowPos(lens.Handle, NativeMethods.HWND_TOPMOST, 0, 0, 0, 0,
                (int)SetWindowPosFlags.SWP_NOACTIVATE | (int)SetWindowPosFlags.SWP_NOMOVE | (int)SetWindowPosFlags.SWP_NOSIZE);

            // Force redraw.
            //NativeMethods.InvalidateRect(hwndMag, IntPtr.Zero, true);

            this.lens.Left = mousePoint.x - lens.Width / 2;
            this.lens.Top = mousePoint.y - lens.Height / 2;

            this.bullseye.Left = mousePoint.x - this.bullseye.Width / 2; 
            this.bullseye.Top = mousePoint.y - this.bullseye.Height / 2; 
        }

        public float Magnification
        {
            get { return magnification; }
            set
            {
                if (magnification != value)
                {
                    magnification = value;
                    // Set the magnification factor.
                    Transformation matrix = new Transformation(magnification);
                    NativeMethods.MagSetWindowTransform(hwndMag, ref matrix);
                }
            }
        }

        protected void SetupMagnifier()
        {
            if (!initialized)
                return;

            IntPtr hInst;

            hInst = NativeMethods.GetModuleHandle(null);

            // Make the window opaque.
            lens.AllowTransparency = true;
            lens.TransparencyKey = System.Drawing.Color.White;
            lens.Opacity = 255;

            // Create a magnifier control that fills the client area.
            NativeMethods.GetClientRect(lens.Handle, ref magWindowRect);
            hwndMag = NativeMethods.CreateWindow((int)ExtendedWindowStyles.WS_EX_TRANSPARENT, NativeMethods.WC_MAGNIFIER,
                "MagnifierWindow", (int)WindowStyles.WS_CHILD | (int)WindowStyles.WS_VISIBLE,
                magWindowRect.left, magWindowRect.top, magWindowRect.right, magWindowRect.bottom, lens.Handle, IntPtr.Zero, hInst, IntPtr.Zero);

            if (hwndMag == IntPtr.Zero)
            {
                return;
            }

            // Set the magnification factor.
            Transformation matrix = new Transformation(magnification);
            NativeMethods.MagSetWindowTransform(hwndMag, ref matrix);
        }

        protected void RemoveMagnifier()
        {
            if (initialized)
                NativeMethods.MagUninitialize();
        }

        protected virtual void Dispose(bool disposing)
        {
            timer.Enabled = false;
            if (disposing)
                timer.Dispose();
            timer = null;
            lens.Resize -= form_Resize;
            RemoveMagnifier();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
