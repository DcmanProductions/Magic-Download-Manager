using com.drewchaseproject.MDM.Library.Utilities;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages.Settings_Sections
{
    /// <summary>
    /// Interaction logic for DownloadSettingsSection.xaml
    /// </summary>
    public partial class CacheSettingsSection : Page
    {
        public CacheSettingsSection()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
        }

        private void RegisterEvents()
        {
            ClearConfigBtn.Click += (s, e) =>
            {
                FileUtilities.ClearConfig();
                MainWindow.Singleton.Main.Content = new Settings();
            };
            ClearLogsBtn.Click += (s, e) => FileUtilities.ClearLogs();
        }

    }
}
