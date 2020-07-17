using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Utilities;
using com.drewchaseproject.MDM.WPF.Pages.Template;
using System;
using System.Windows;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Downloads.xaml
    /// </summary>
    public partial class Downloads : Page
    {
        private readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        private static Downloads _singleton;
        public static Downloads Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new Downloads();
                }
                //Downloads.Singleton.LoadDownloads();
                return _singleton;
            }
        }

        private Downloads()
        {
            InitializeComponent();
            RegisterEvents();
            Values.Singleton.DownloadView = DownloadViewer;
            //UIUtility.GenerateDownloadUI(DownloadViewer);
            LoadDownloads();
        }

        public void LoadDownloads()
        {
            DownloadViewer.Children.Clear();
            Values.Singleton.DownloadQueue.ForEach((n) => DownloadViewer.Children.Add(new Frame() { Content = new DownloadTemplate(n) }));
        }

        private void RegisterEvents()
        {
            AddDownloadBtn.Click += ( (object sender, RoutedEventArgs e) =>
            {
                MainWindow.Singleton.Main.Content = new AddDownload();
            } );
            PlayDownloadBtn.Click += (s, e) =>
            {
                if (Values.Singleton.DownloadQueue.Count > 0)
                {
                    try
                    {
                        DownloadFile d = Values.Singleton.DownloadQueue[0];
                        d.IsDownloading = true;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unknown Error Occurred While Clicking Play Download Button", ex);
                    }
                }
            };
        }
    }
}
