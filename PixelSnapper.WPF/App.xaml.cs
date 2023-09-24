using PixelSnapper.Hotkey;
using PixelSnapper.WPF.Services;
using PixelSnapper.WPF.Utilities;
using PixelSnapper.WPF.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace PixelSnapper.WPF
{
    public partial class App : System.Windows.Application
    {
        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;
        private ToolStripMenuItem SettingsMenuItem;

        private Settings settingsWindow;

        public App()
        { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var globalHook = new KeyboardHook(true);
            var settingsService = new SettingsService();
            settingsService.Load();

            ServiceLocator.Register(settingsService);
            ServiceLocator.Register(globalHook);

            settingsWindow = new Settings();
            var lens = new Lens();

            ServiceLocator.Register(lens);
            ServiceLocator.Register(settingsWindow);
            ServiceLocator.Register(new HotkeyService(settingsService, globalHook, settingsWindow, lens));

            ConfigureTrayIcon();

            if (settingsService.IsFirstRun)
            {
                FirstRun firstRunPopup = new FirstRun();
                firstRunPopup.Show();

                //Create settings file in order to prevent future popups.
                settingsService.Save();
            }
        }

        private void ConfigureTrayIcon()
        {
            TrayIcon = new NotifyIcon();

            using (Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/PixelSnapper.WPF;component/Images/TrayIcon.ico")).Stream)
            {
                TrayIcon.Icon = new System.Drawing.Icon(iconStream);
            }

            TrayIcon.DoubleClick += ShowSettings_Click;

            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            SettingsMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] { this.SettingsMenuItem, this.CloseMenuItem });
            this.TrayIconContextMenu.Name = "ColorSnapContextMenu";
            this.TrayIconContextMenu.Size = new System.Drawing.Size(153, 70);

            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SettingsMenuItem.Text = "Settings";
            this.SettingsMenuItem.Click += new EventHandler(this.ShowSettings_Click);

            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new System.Drawing.Size(152, 22);
            this.CloseMenuItem.Text = "Exit";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;

            System.Windows.Forms.Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            TrayIcon.Visible = true;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
        }

        private void ShowSettings_Click(object sender, EventArgs e)
        {
            this.settingsWindow.Show();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Current.Shutdown();
        }

    }
}
