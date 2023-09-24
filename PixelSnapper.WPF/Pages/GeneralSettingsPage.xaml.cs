using Microsoft.Win32;
using System.Windows.Controls;
using System.Linq;

namespace PixelSnapper.WPF.Pages
{
    public partial class GeneralSettingsPage : UserControl
    {
        public GeneralSettingsPage()
        {
            InitializeComponent();
            chkRunAtStartup.IsChecked = GetStartup();            
        }

        private bool GetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            return rk.GetValueNames().Contains("ColorPicker");
        }

        private void SetStartup(bool? set)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (set == true)
                    rk.SetValue("ColorPicker", System.Windows.Forms.Application.ExecutablePath.ToString());
                else
                    rk.DeleteValue("ColorPicker", false);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error setting application auto-startup. Running the application as administrator might resolve this problem.");
            }
        }

        private void RunOnStartup_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SetStartup((sender as CheckBox).IsChecked);
        }
    }
}
