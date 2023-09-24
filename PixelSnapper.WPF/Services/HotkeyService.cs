using PixelSnapper.Hotkey;
using PixelSnapper.WPF.Windows;
using System.Linq;

namespace PixelSnapper.WPF.Services
{
    public class HotkeyService
    {
        private SettingsService settingsService;
        private KeyboardHook hotkeysHook;
        private Settings settingsWindow;
        private Lens picker;

        private KeyCombination startCapturing, cancelCapturing;

        public HotkeyService(SettingsService settingsService, KeyboardHook globalHook, Settings settingsWindow, Lens picker)
        {
            this.settingsService = settingsService;
            this.settingsWindow = settingsWindow;
            this.hotkeysHook = globalHook;
            this.picker = picker;

            RefreshKeybindings();
            hotkeysHook.KeyDown += HotkeysHook_KeyDown;
        }

        public void RefreshKeybindings()
        {
            startCapturing = settingsService.Current.Hotkeys.FirstOrDefault(s => s.Tag == "StartCapture").GetHotkey();
            cancelCapturing = settingsService.Current.Hotkeys.FirstOrDefault(s => s.Tag == "CancelCapture").GetHotkey();
        }

        private void HotkeysHook_KeyDown(KeyCombination combo)
        {
            if (settingsWindow.IsConfiguringKeys)
            {
                settingsWindow.GlobalKeyDown(combo);
            }
            else
            {
                if (startCapturing.Equals(combo))
                {
                    picker.Show();
                }

                if (cancelCapturing.Equals(combo))
                {
                    picker.Hide();
                }
            }
        }
    }
}
