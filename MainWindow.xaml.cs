using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AfterDarkness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetSystemTheme(bool isDarkMode)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true))
            {
                if (key != null)
                {
                    key.SetValue("AppsUseLightTheme", isDarkMode ? 0 : 1, RegistryValueKind.DWord);
                    key.SetValue("SystemUsesLightTheme", isDarkMode ? 0 : 1, RegistryValueKind.DWord);
                }
            }

            // Restart Explorer to apply changes
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C taskkill /F /IM explorer.exe & start explorer.exe",
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }

        private void UpdateThemeButtons()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                var appsUseLightTheme = key.GetValue("AppsUseLightTheme");
                if (appsUseLightTheme != null && (int)appsUseLightTheme == 0)
                {
                    DarkModeButton.IsEnabled = false;
                    LightModeButton.IsEnabled = true;
                }
                else
                {
                    DarkModeButton.IsEnabled = true;
                    LightModeButton.IsEnabled = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetSystemTheme(false);
        }

        private void DarkModeButton_Click(object sender, RoutedEventArgs e)
        {
            SetSystemTheme(true);
        }
    }
}