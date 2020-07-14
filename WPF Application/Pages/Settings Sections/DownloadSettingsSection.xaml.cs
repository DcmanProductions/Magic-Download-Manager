using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages.Settings_Sections
{
    /// <summary>
    /// Interaction logic for DownloadSettingsSection.xaml
    /// </summary>
    public partial class DownloadSettingsSection : Page
    {
        public DownloadSettingsSection()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
            SplitTextBox.Text = Values.Singleton.FileSplitCount + "";
            ConnectionsTextBox.Text = Values.Singleton.ConnectionsPerProxy + "";
            UIUtility.SetCheckBox(PreAllocateStorageCheckBtn, Values.Singleton.PreAllocate);
            DownloadLocationTextBlock.Text = Values.Singleton.DownloadDirectory;
        }

        private void RegisterEvents()
        {
            ConnectionsTextBox.TextChanged += (s, e) =>
             {
                 string text = ConnectionsTextBox.Text;
                 if (int.TryParse(text, out int number))
                 {
                     Values.Singleton.ConnectionsPerProxy = number;
                 }
                 ConnectionsTextBox.Text = Values.Singleton.ConnectionsPerProxy + "";
             };

            SplitTextBox.TextChanged += (s, e) =>
            {
                string text = SplitTextBox.Text;
                if (int.TryParse(text, out int number))
                {
                    Values.Singleton.FileSplitCount = number;
                }
                SplitTextBox.Text = Values.Singleton.FileSplitCount + "";
            };

            PreAllocateStorageCheckBtn.Click += (s, e) => UIUtility.ToggleCheckBox((Button) e.Source);
            DownloadLocationBtn.Click += (s, e) =>
            {
                Values.Singleton.DownloadDirectory = FileUtilities.OpenFolder(Values.Singleton.DownloadDirectory, "Select Download Folder");
                DownloadLocationTextBlock.Text = Values.Singleton.DownloadDirectory;
            };

        }

    }
}
