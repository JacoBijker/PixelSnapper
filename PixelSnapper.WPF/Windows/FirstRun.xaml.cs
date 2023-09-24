using PixelSnapper.WPF.Utilities;
using System.Windows;

namespace PixelSnapper.WPF.Windows
{
    /// <summary>
    /// Interaction logic for FirstRun.xaml
    /// </summary>
    public partial class FirstRun : Window
    {
        public FirstRun()
        {
            InitializeComponent();
        }

        private void GotIt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Resolve<Settings>().Show();
        }
    }
}
