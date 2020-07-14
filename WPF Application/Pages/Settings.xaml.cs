using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.WPF.Pages.Settings_Sections;
using System.Windows;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public Settings()
        {
            log.Debug("Loading Settings Page");
            InitializeComponent();

            LoadPages(new DownloadSettingsSection(), new AccountSettingsSection(), new CacheSettingsSection());

        }

        private void LoadPages(params Page[] pages)
        {
            foreach (Page page in pages)
            {
                log.Debug($"Loading Settings Section {page.Title}");
                SettingsView.Children.Add(new Frame() { Content = page, Margin = new Thickness(0, 0, 0, 50) });
            }
        }

    }
}
