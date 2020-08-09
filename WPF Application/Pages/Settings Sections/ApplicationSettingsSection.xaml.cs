using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages.Settings_Sections
{
    /// <summary>
    /// Interaction logic for ApplicationSettingsSection.xaml
    /// </summary>
    public partial class ApplicationSettingsSection : Page
    {
        public ApplicationSettingsSection()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
            UIUtility.SetCheckBox(StartWithWindowsCheckBtn, Values.Singleton.StartWithWindows);
            UIUtility.SetCheckBox(StartMinimizedCheckBtn, Values.Singleton.MinimizeOnStart);
            UIUtility.SetCheckBox(MaximizeOnDownloadCheckBtn, Values.Singleton.MaximizeOnDownload);
        }

        private void RegisterEvents()
        {
            StartWithWindowsCheckBtn.Click += (s, e) => Values.Singleton.StartWithWindows = UIUtility.ToggleCheckBox((Button) s);
            StartMinimizedCheckBtn.Click += (s, e) => Values.Singleton.MinimizeOnStart = UIUtility.ToggleCheckBox((Button) s);
            MaximizeOnDownloadCheckBtn.Click += (s, e) => Values.Singleton.MaximizeOnDownload = UIUtility.ToggleCheckBox((Button) s);
        }

    }
}
