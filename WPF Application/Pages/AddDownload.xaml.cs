using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
            Update();
            CheckValidDownload();
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
                CheckValidDownload();
            };

            DownloadLocationBtn.Click += (s, e) =>
            {
                FileUtilities.OpenFolder(DownloadLocationTextBlock.Text, "Select Download Location for This File");
                CheckValidDownload();
            };

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
                CheckValidDownload();
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
                CheckValidDownload();
                SplitTextBox.Text = split + "";
            };

            URLTextBox.LostFocus += (s, e) =>
            {
                CheckValidDownload();
            };

            AddDownloadBtn.Click += (s, e) =>
            {
                Added = true;
                if (FileUtilities.IsSingleFile(URLTextBox.Text))
                    Downloads.Singleton.AddDownload(FileUtilities.ImportDownloads(URLTextBox.Text).ToArray());
                else if (NetworkUtility.IsValidDownloadUrl(URLTextBox.Text))
                    Downloads.Singleton.AddDownload(GetDownloadFile);
                MainWindow.Singleton.Main.Content = Downloads.Singleton;
            };

            AddDownloadTextFile.Click += (s, e) =>
            {
                URLTextBox.Text = FileUtilities.OpenFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Select Download File URL");
                CheckValidDownload();
            };

        }

        private void Update()
        {
            Dispatcher dis = Values.Singleton.MainDispatcher;
            double seconds = 2;
            long currentTime = DateTime.Now.Ticks, neededTime = 0;
            neededTime = DateTime.Now.AddSeconds(seconds).Ticks;
            Task.Run(() =>
            {
                while (true)
                {
                    currentTime = DateTime.Now.Ticks;
                    if (currentTime >= neededTime)
                    {
                        dis.Invoke(new Action(() =>
                        {
                            CheckValidDownload();
                        }), DispatcherPriority.ContextIdle);
                        neededTime = DateTime.Now.AddSeconds(seconds).Ticks;
                    }
                }
            });
        }

        private void CheckValidDownload()
        {

            if (!string.IsNullOrEmpty(URLTextBox.Text) && ( NetworkUtility.IsValidDownloadUrl(URLTextBox.Text) || FileUtilities.IsSingleFile(URLTextBox.Text) ) && ( UIUtility.GetCheckBox(UseGlobalCheckBtn) || FileUtilities.IsValidPath(DownloadLocationTextBlock.Text) ))
            {
                AddDownloadBtn.Visibility = Visibility.Visible;
            }
            else
            {
                AddDownloadBtn.Visibility = Visibility.Collapsed;
            }
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
