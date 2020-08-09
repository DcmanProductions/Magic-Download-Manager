using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.WPF.Pages.Template;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            Setup();
            RegisterEvents();
            Update();
        }

        private void Setup()
        {
            Values.Singleton.PlayDownloadBtn = PlayDownloadBtn;
            Values.Singleton.DownloadViewer = DownloadViewer;
            Values.Singleton.DownloadPageTitle = PageTitle;
            StopDownloadBtn.IsEnabled = false;
            LoadDownloads();

        }
        public void LoadDownloads()
        {
            Values.Singleton.DownloadViewer.Children.Clear();
            Values.Singleton.DownloadQueue.ForEach((n) => Values.Singleton.DownloadViewer.Children.Add(new Frame() { Name = n.ComponentName, Content = new DownloadTemplate(n) }));
        }

        public void AddDownload(params DownloadFile[] files)
        {
            try
            {
                if (Values.Singleton.MaximizeOnDownload)
                {
                    if (MainWindow.Singleton.WindowState == WindowState.Minimized)
                    {
                        MainWindow.Singleton.WindowState = WindowState.Normal;
                    }

                    MainWindow.Singleton.Activate();
                    MainWindow.Singleton.Main.Content = this;
                }
                else
                {
                    if (MainWindow.Singleton.WindowState == WindowState.Minimized)
                    {
                        MainWindow.Singleton.NotifyIcon.BalloonTipTitle = "Download Added...";
                        MainWindow.Singleton.NotifyIcon.BalloonTipText = "Right Click to See more options";
                        MainWindow.Singleton.NotifyIcon.ShowBalloonTip(400);
                        MainWindow.Singleton.NotifyIcon.BalloonTipClicked += (s, e) =>
                        {
                            MainWindow.Singleton.WindowState = WindowState.Normal;
                            MainWindow.Singleton.Activate();
                            MainWindow.Singleton.Main.Content = this;
                        };
                    }
                }
            }
            catch (Exception e)
            {
                if (MainWindow.Singleton.Main.Content == null)
                {
                    log.Error("Content was null, ignoring", e);
                }
                else
                {
                    log.Error("Unknown Exception while trying to focus on download window while adding download", e);
                }
            }
            Values.Singleton.DownloadQueue.AddRange(files);
            files.ToList().ForEach((n) =>
            {
                Values.Singleton.DownloadViewer.Children.Add(new Frame()
                {

                    Name = n.ComponentName,
                    Content = new DownloadTemplate(n)
                });
            });
        }

        private void RegisterEvents()
        {
            AddDownloadBtn.Click += (s, e) => MainWindow.Singleton.Main.Content = new AddDownload();

            ClearDownloadBtn.Click += (s, e) => ClearDownloadQueue();

            StopDownloadBtn.Click += (s, e) => StopDownloadQueue();

            PlayDownloadBtn.Click += (s, e) => PlayDownloadQueue();
        }

        private void PlayDownloadQueue()
        {
            if (Values.Singleton.DownloadQueue.Count > 0)
            {
                Values.Singleton.DownloadPageTitle.Content = $"{Values.Singleton.DownloadQueue.Count} Remaining";
            }

            if (Values.Singleton.DownloadQueue.Count > 0)
            {
                try
                {
                    DownloadFile d = Values.Singleton.DownloadQueue[0];
                    d.IsDownloading = true;
                    StopDownloadBtn.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    log.Error("Unknown Error Occurred While Clicking Play Download Button", ex);
                }
            }
        }

        private void ClearDownloadQueue()
        {
            if (Values.Singleton.CurrentFileDownloading != null)
            {
                Values.Singleton.CurrentFileDownloading.IsDownloading = false;
            }

            PlayDownloadBtn.IsEnabled = true;
            StopDownloadBtn.IsEnabled = false;
            Values.Singleton.DownloadQueue.Clear();
            LoadDownloads();
            PageTitle.Content = "Downloads";
        }

        private void StopDownloadQueue()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Values.Singleton.CurrentFileDownloading != null)
                {
                    Values.Singleton.CurrentFileDownloading.IsDownloading = false;
                    Values.Singleton.CurrentFileDownloading = null;
                }
            }
            PlayDownloadBtn.IsEnabled = true;
            StopDownloadBtn.IsEnabled = false;
            PageTitle.Content = "Downloads";
        }

        private void HotSwapDownloads()
        {
            if (!File.Exists(Values.Singleton.HotSwapDownloadCache))
            {
                return;
            }

            try
            {

                using (StreamReader reader = new StreamReader(Values.Singleton.HotSwapDownloadCache))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        //if (NetworkUtility.IsValidDownloadUrl(line))
                        AddDownload(new DownloadFile() { URL = line });
                    }
                }

                if (Values.Singleton.DownloadQueue.Count > 0 && !Values.Singleton.DownloadQueue[0].IsDownloading)
                {
                    PlayDownloadQueue();
                }
            }
            catch (Exception e)
            {
                log.Error("Unexpected Error while Loading Hotswap cache file", e);
            }
            try
            {
                File.Delete(Values.Singleton.HotSwapDownloadCache);
            }
            catch (IOException e)
            {
                log.Error("Could NOT Remove the hotswap cache File", e);
            }
            catch (Exception e)
            {
                log.Error("Unexpected Error while attempting to remove the hotswap cache file", e);
            }
        }

        private void Update()
        {
            System.Windows.Threading.Dispatcher dis = Values.Singleton.MainDispatcher;
            double seconds = 1;
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
                            HotSwapDownloads();
                        }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                        neededTime = DateTime.Now.AddSeconds(seconds).Ticks;
                    }
                }
            });
        }

    }
}
