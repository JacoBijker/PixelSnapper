using PixelSnapper.Hotkey;
using PixelSnapper.WPF.Pages;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace PixelSnapper.WPF.Windows
{
    public partial class Settings : Window
    {
        public UserControl GeneralSettingsPage { get; set; }

        public KeybindingsPage KeybindingsPage { get; set; }

        public UserControl ColorFormattingPage { get; set; }

        public bool IsConfiguringKeys
        {
            get
            {
                return this.KeybindingsPage.IsConfiguringKeys;
            }
        }

        public Settings()
        {
            InitializeComponent();

            this.GeneralSettingsPage = new GeneralSettingsPage();
            this.ColorFormattingPage = new ColorFormattingPage();
            this.KeybindingsPage = new KeybindingsPage();

            this.DataContext = this;
        }

        public void GlobalKeyDown(KeyCombination combo)
        {
            KeybindingsPage.GlobalKeyDown(combo);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
            base.OnClosing(e);
        }
    }
}
