using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using com.drewchaseproject.MDM.Library.Utilities;
using com.drewchaseproject.MDM.WPF.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using Application = System.Windows.Application;

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
            //Task.Run(() => AutoCheckForUpdates());
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
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 9;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth - 9;
            Values.Singleton.MainDispatcher = Application.Current.Dispatcher;
            if (File.Exists(Values.Singleton.LogFileLocation))
            {
                File.Delete(Values.Singleton.LogFileLocation);
            }

            ChangeView(PageType.Welcome);
            Values.Singleton.DownloadQueue.AddRange(FileUtilities.ImportDownloads());
            _ = Configuration.Singleton;
            _ = Downloads.Singleton;

            Values.Singleton.CurrentlyExecutingApplicationAssembly = Assembly.GetExecutingAssembly();
            Values.Singleton.StartWithWindows = RegistryUtility.IsAddedToStartup;
            if (!RegistryUtility.IsUrlProtocolAdded)
            {
                RegistryUtility.AddUrlProtocol();
            }
        }

        private void RegisterEvents()
        {
            SystemTray();
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

        public void CheckAuth()
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

        public void Logout()
        {
            Values.Singleton.Activated = false;
            MenuBar.Visibility = Visibility.Collapsed;
            ChangeView(PageType.Settings);
        }

        private void PreClose()
        {
            OnExit();
            Close();
        }

        private void OnExit()
        {
            if (NotifyIcon != null)
            {
                NotifyIcon.Visible = false;
            }

            FastDownloadExecutableUtility.DestroyExecutable();
            try
            {
                if (Values.Singleton.CurrentFileDownloading != null && Values.Singleton.CurrentFileDownloading.DownloadFileProcess != null && !Values.Singleton.CurrentFileDownloading.DownloadFileProcess.HasExited)
                {
                    Values.Singleton.CurrentFileDownloading.DownloadFileProcess.Kill();
                }
            }
            catch { }

            FileUtilities.ExportDownloads();

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

        private void DelayedSetup()
        {
            long current = DateTime.Now.Ticks, wanted = DateTime.Now.AddSeconds(2).Ticks;
            while (current < wanted)
            {
                current = DateTime.Now.Ticks;
            }
            if (Values.Singleton.MinimizeOnStart)
            {
                WindowState = WindowState.Minimized;
                ShowInTaskbar = false;
                NotifyIcon.BalloonTipTitle = "SUckme day";
                NotifyIcon.BalloonTipText = "Right Click to See more options";
                NotifyIcon.ShowBalloonTip(400);
                NotifyIcon.Visible = true;
            }
        }

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        public System.Windows.Forms.NotifyIcon NotifyIcon;
        private void SystemTray()
        {
            NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location)
            };
            NotifyIcon.Visible = true;
            StateChanged += (s, e) =>
           {
               if (WindowState == WindowState.Minimized)
               {
                   ShowInTaskbar = false;
                   NotifyIcon.BalloonTipTitle = "Minimized Magic DM";
                   NotifyIcon.BalloonTipText = "Right Click to See more options";
                   NotifyIcon.ShowBalloonTip(400);
                   NotifyIcon.Visible = true;
               }
               else if (WindowState == WindowState.Normal)
               {
                   NotifyIcon.Visible = true;
                   ShowInTaskbar = true;
                   Activate();
               }
           };

            contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.Items.Add("Show", null, new EventHandler((object sender, EventArgs args) => { WindowState = WindowState.Normal; }));
            //contextMenu.Items.Add("Check For Updates", null, new EventHandler((object sender, EventArgs args) => { CheckForUpdates(); }));
            contextMenu.Items.Add("Exit", null, new EventHandler((object sender, EventArgs args) => { PreClose(); }));
            NotifyIcon.ContextMenuStrip = contextMenu;
        }

        private void CheckForUpdates()
        {
            if (Values.Singleton.UpdateAvailable)
            {
                contextMenu.Items.Add("Update Available", null, new EventHandler((object sender, EventArgs args) =>
                {
                    new Process() { StartInfo = new ProcessStartInfo() { FileName = Values.Singleton.LauncherExe } }.Start();
                    PreClose();
                }));
            }
        }

        private void AutoCheckForUpdates()
        {
            int seconds = 5;
            int minutes = 20;
            long current = DateTime.Now.Ticks, wanted = DateTime.Now.AddMinutes(minutes).Ticks, test = DateTime.Now.AddSeconds(seconds).Ticks;
            while (current < wanted)
            {
                current = DateTime.Now.Ticks;
                //if (current >= wanted)
                //{
                //    wanted = DateTime.Now.AddMinutes(minutes).Ticks;
                //    CheckForUpdates();
                //}
                if (current >= test)
                {
                    test = DateTime.Now.AddSeconds(test).Ticks;
                    CheckForUpdates();
                }
            }
        }

    }
}
