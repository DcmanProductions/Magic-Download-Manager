using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using com.drewchaseproject.MDM.Library.Utilities;
using com.drewchaseproject.MDM.WPF.Pages;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace com.drewchaseproject.MDM.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        private static MainWindow _singleton;
        public static MainWindow Singleton => _singleton;

        public MainWindow()
        {
            InitializeComponent();
            _singleton = this;
            Setup();
            RegisterEvents();

            CheckAuth();

        }

        public enum PageType
        {
            Welcome,
            Downloads,
            Console,
            Settings,
            AddDownload
        }

        private void Setup()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight-9;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth-9;
            Values.Singleton.MainDispatcher = Application.Current.Dispatcher;
            if (File.Exists(Values.Singleton.LogFileLocation))
            {
                File.Delete(Values.Singleton.LogFileLocation);
            }

            ChangeView(PageType.Welcome);
            _ = Configuration.Singleton;
        }

        private void RegisterEvents()
        {
            //SourceInitialized += (s, e) =>
            //{
            //    IntPtr handle = ( new WindowInteropHelper(this) ).Handle;
            //    HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(UIUtility.WindowProc));
            //};

            MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                    }

                    DragMove();
                }
            };

            MenuViewBtn.Click += ( (object sender, RoutedEventArgs e) => ChangeView(PageType.Welcome) );
            SettingsViewBtn.Click += ( (object sender, RoutedEventArgs e) => ChangeView(PageType.Settings) );
            LogViewBtn.Click += ( (object sender, RoutedEventArgs e) => ChangeView(PageType.Console) );
            DownloadViewBtn.Click += ( (object sender, RoutedEventArgs e) => ChangeView(PageType.Downloads) );

            MinimizeBtn.Click += ( (s, e) => WindowState = WindowState.Minimized );
            MaximizeBtn.Click += ( (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized );
            CloseBtn.Click += ( (s, e) => PreClose() );


            AppDomain.CurrentDomain.ProcessExit += ( (object sender, EventArgs e) => OnExit() );

        }

        private void CheckAuth()
        {
            if (!Activation.IsAuthorizedUser(Values.Singleton.Username, Values.Singleton.Password))
            {
                Values.Singleton.Activated = false;
                MenuBar.Visibility = Visibility.Collapsed;
                ChangeView(PageType.Settings);
            }
            else
            {
                Values.Singleton.Activated = true;
            }
        }

        private void PreClose()
        {
            OnExit();
            Close();
        }

        private void OnExit()
        {
            FastDownloadExecutableUtility.DestroyExecutable();
            try
            {
                if (Values.Singleton.CurrentFileDownloading != null && Values.Singleton.CurrentFileDownloading.DownloadFileProcess != null && !Values.Singleton.CurrentFileDownloading.DownloadFileProcess.HasExited)
                    Values.Singleton.CurrentFileDownloading.DownloadFileProcess.Kill();
            }
            catch { }
        }

        public void ChangeView(PageType page)
        {
            switch (page)
            {
                case PageType.Welcome:
                    Main.Content = new Welcome();
                    break;
                case PageType.Downloads:
                    Main.Content = Downloads.Singleton;
                    break;
                case PageType.Settings:
                    Main.Content = new Settings();
                    break;
                case PageType.Console:
                    Main.Content = new Log();
                    break;
                default:
                    ChangeView(PageType.Welcome);
                    break;
            }
        }


    }
}
