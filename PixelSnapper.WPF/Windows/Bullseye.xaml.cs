using PixelSnapper.Magnification;
using PixelSnapper.WPF.Utilities;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PixelSnapper.WPF
{
    public partial class Bullseye : Window
    {
        private Timer timer, closeTimer;
        private Storyboard scaleUpAnimation, scaleDownAnimation;

        public Bullseye()
        {
            InitializeComponent();

            scaleUpAnimation = FindResource("scaleUpAnimation") as Storyboard;
            scaleDownAnimation = FindResource("scaleDownAnimation") as Storyboard;
            Storyboard.SetTarget(scaleUpAnimation, selectedColor);
            Storyboard.SetTarget(scaleDownAnimation, selectedColor);


            closeTimer = new Timer();
            closeTimer.Elapsed += CloseTimer_Elapsed;
            closeTimer.Interval = 200;

            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 50;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                POINT mousePoint = new POINT();
                NativeMethods.GetCursorPos(ref mousePoint);
                var color = Color.ColorGrabber.GetColorAt(mousePoint.x, mousePoint.y);

                var solidColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, color.R, color.G, color.B));
                ellipse.Stroke = solidColor;
                palette.Fill = solidColor;
            }));
        }

        public new void Show()
        {
            base.Show();
            timer.Start();
        }

        public new void Hide()
        {
            base.Hide();
            timer.Stop();
        }

        public void SnapColor()
        {
            selectedColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorHolder.CurrentColor.R, ColorHolder.CurrentColor.G, ColorHolder.CurrentColor.B));
            scaleUpAnimation.Begin();
            closeTimer.Start();
        }

        private void CloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            closeTimer.Stop();

            Dispatcher.BeginInvoke((Action)(() =>
            {
                Hide();
                scaleDownAnimation.Begin();
            }));
        }
    }
}
