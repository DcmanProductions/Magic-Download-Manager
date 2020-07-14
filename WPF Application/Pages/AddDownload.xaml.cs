using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for AddDownload.xaml
    /// </summary>
    public partial class AddDownload : Page
    {
        private int proxy = 0, split = 0;
        public AddDownload()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
            Added = false;
            UIUtility.SetCheckBox(UseGlobalCheckBtn, true);
            LocalDownloadSettings.Visibility = Visibility.Collapsed;

            DownloadLocationTextBlock.Text = Values.Singleton.DownloadDirectory;
            UIUtility.SetCheckBox(PreAllocateStorageCheckBtn, Values.Singleton.PreAllocate);
            SplitTextBox.Text = Values.Singleton.FileSplitCount + "";
            ConnectionsTextBox.Text = Values.Singleton.ConnectionsPerProxy + "";
            split = Values.Singleton.FileSplitCount;
            proxy = Values.Singleton.ConnectionsPerProxy;

        }

        private void RegisterEvents()
        {
            UseGlobalCheckBtn.Click += (s, e) =>
            {
                if (UIUtility.ToggleCheckBox(UseGlobalCheckBtn))
                {
                    LocalDownloadSettings.Visibility = Visibility.Collapsed;
                }
                else
                {
                    LocalDownloadSettings.Visibility = Visibility.Visible;
                }
            };

            DownloadLocationBtn.Click += (s, e) => FileUtilities.OpenFolder(DownloadLocationTextBlock.Text, "Select Download Location for This File");

            ConnectionsTextBox.TextChanged += (s, e) =>
            {
                string text = ConnectionsTextBox.Text;
                if (!int.TryParse(text, out proxy))
                {
                    proxy = Values.Singleton.FileSplitCount;
                }
                else
                {
                    if (proxy < 1)
                    {
                        proxy = 1;
                    }

                    if (proxy > 16)
                    {
                        proxy = 16;
                    }
                }
                ConnectionsTextBox.Text = proxy + "";
            };

            SplitTextBox.TextChanged += (s, e) =>
            {
                string text = SplitTextBox.Text;
                if (!int.TryParse(text, out split))
                {
                    split = Values.Singleton.FileSplitCount;
                }
                else
                {
                    if (split < 1)
                    {
                        split = 1;
                    }
                }
                SplitTextBox.Text = split + "";
            };

            AddDownloadBtn.Click += (s, e) =>
            {
                Added = true;
                UIUtility.GenerateDownloadUI(Downloads.Singleton.DownloadViewer, GetDownloadFile);
                MainWindow.Singleton.Main.Content = Downloads.Singleton;
            };

        }

        public bool Added { get; private set; }

        public DownloadFile GetDownloadFile
        {
            get
            {
                if (UIUtility.GetCheckBox(UseGlobalCheckBtn))
                {
                    return new DownloadFile()
                    {
                        URL = URLTextBox.Text,
                        DownloadLocation = Values.Singleton.DownloadDirectory,
                        MaxSplitSize = Values.Singleton.FileSplitCount,
                        Proxys = Values.Singleton.ConnectionsPerProxy,
                        CurrentProgress = 0
                    };
                }
                else
                {
                    return new DownloadFile()
                    {
                        URL = URLTextBox.Text,
                        DownloadLocation = DownloadLocationTextBlock.Text,
                        MaxSplitSize = split,
                        Proxys = proxy,
                        CurrentProgress = 0
                    };
                }
            }
        }
    }
}
