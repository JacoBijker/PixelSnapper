using PixelSnapper.Converters;
using PixelSnapper.GobalInjections;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace PixelSnapper.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int interval = 40;
        private Timer colorGrabInterval;
        private HotKey currentHotkey;

        private SolidColorBrush hoverColor;
        public SolidColorBrush HoverColor
        {
            get
            {
                return hoverColor;
            }
            set
            {
                if (hoverColor == value)
                    return;

                hoverColor = value;
                hoverColor.Freeze();
                OnPropertyChanged();
            }
        }

        public System.Drawing.Color rawColor;
        public System.Drawing.Color RawColor
        {
            get
            {
                return rawColor;
            }
            set
            {
                if (rawColor == value)
                    return;

                rawColor = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush capturedColor;
        public SolidColorBrush CapturedColor
        {
            get
            {
                return capturedColor;
            }
            set
            {
                capturedColor = value;
                OnPropertyChanged();
            }
        }

        private string clipboardValue = "Ctrl+F1 to set";
        public string ClipboardValue
        {
            get
            {
                return clipboardValue;
            }
            set
            {
                if (clipboardValue == value)
                    return;

                clipboardValue = value;
                OnPropertyChanged();
            }
        }

        private bool storyboardStart;
        public bool StoryboardStart
        {
            get { return storyboardStart; }
            set
            {
                storyboardStart = value;
                OnPropertyChanged();
            }
        }


        public ApplicationViewModel()
        {
            colorGrabInterval = new Timer(interval);
            colorGrabInterval.Elapsed += OnColorGrab;
            colorGrabInterval.Enabled = true;

            currentHotkey = new HotKey(System.Windows.Input.Key.F1, KeyModifier.Ctrl, OnHotkey);
        }

        private void OnHotkey(HotKey key)
        {
            ColorToHEXStringConverter cnv = new ColorToHEXStringConverter();
            var hexResult = cnv.Convert(RawColor, null, null, null).ToString();
            ClipboardValue = hexResult;
            Clipboard.SetText(ClipboardValue);

            CapturedColor = new SolidColorBrush(Color.FromRgb(RawColor.R, RawColor.G, RawColor.B));

            StoryboardStart = false;
            StoryboardStart = true;
        }

        private void OnColorGrab(object source, ElapsedEventArgs e)
        {
            var currentCursorPos = MouseGrabber.GetCursorPosition();
            RawColor = ColorGrabber.GetColorAt(currentCursorPos);

            var wpfColor = Color.FromRgb(RawColor.R, RawColor.G, RawColor.B);
            HoverColor = new SolidColorBrush(wpfColor);
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            if (colorGrabInterval != null)
                colorGrabInterval.Elapsed -= OnColorGrab;

            currentHotkey.Unregister();
            colorGrabInterval = null;
        }
    }
}
