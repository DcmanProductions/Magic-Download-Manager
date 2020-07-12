using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        void Setup()
        {
            SplitTextBox.Text = Values.Singleton.FileSplitCount + "";
            ConnectionsTextBox.Text = Values.Singleton.ConnectionsPerProxy + "";
            UIUtility.SetCheckBox(PreAllocateStorageCheckBtn, Values.Singleton.PreAllocate);
            DownloadLocationTextBlock.Text = Values.Singleton.DownloadDirectory;
        }

        void RegisterEvents()
        {
            ConnectionsTextBox.TextChanged += (s, e) =>
             {
                 string text = ConnectionsTextBox.Text;
                 int number = 0;
                 if (int.TryParse(text, out number))
                 {
                     Values.Singleton.ConnectionsPerProxy = number;
                 }
                 ConnectionsTextBox.Text = Values.Singleton.ConnectionsPerProxy + "";
             };

            SplitTextBox.TextChanged += (s, e) =>
            {
                string text = SplitTextBox.Text;
                int number = 0;
                if (int.TryParse(text, out number))
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
