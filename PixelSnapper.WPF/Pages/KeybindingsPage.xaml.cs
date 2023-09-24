using PixelSnapper.Hotkey;
using PixelSnapper.WPF.Services;
using PixelSnapper.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace PixelSnapper.WPF.Pages
{
    public partial class KeybindingsPage : UserControl
    {
        private Dictionary<string, KeyCombination> currentHotkeys;
        private TextBox currentTextBox;

        public bool IsConfiguringKeys { get; set; }

        public KeybindingsPage()
        {
            InitializeComponent();
            LoadHotkeys();
        }

        private void LoadHotkeys()
        {
            currentHotkeys = new Dictionary<string, KeyCombination>();

            try
            {
                var settings = ServiceLocator.Resolve<SettingsService>();

                foreach (var item in settings.Current.Hotkeys)
                    currentHotkeys[item.Tag] = item.GetHotkey();
            }
            catch (Exception ex)
            { }

            tbStartCapture.Text = currentHotkeys[tbStartCapture.Tag.ToString()].ToString();
            tbCancelCapture.Text = currentHotkeys[tbCancelCapture.Tag.ToString()].ToString();
        }

        internal void GlobalKeyDown(KeyCombination combo)
        {
            currentHotkeys[currentTextBox.Tag.ToString()] = combo;
            currentTextBox.Text = combo.ToString();
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.currentTextBox = (TextBox)sender;
            IsConfiguringKeys = true;
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IsConfiguringKeys = false;
        }

        private void ApplyChanges_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var settings = ServiceLocator.Resolve<SettingsService>();
            settings.SetHokeys(currentHotkeys);
            settings.Save();

            ServiceLocator.Resolve<HotkeyService>().RefreshKeybindings();
        }

        private void Undo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadHotkeys();
        }
    }
}
