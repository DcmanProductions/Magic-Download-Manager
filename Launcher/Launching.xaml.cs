using ChaseLabs.CLUpdate;
using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using com.drewchaseproject.MDM.Library.Objects;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for Launching.xaml
    /// </summary>
    public partial class Launching : Page
    {
        private readonly Dispatcher dis = Dispatcher.CurrentDispatcher;
        public Launching()
        {
            InitializeComponent();
            Auth();
        }

        private void Auth()
        {
            if (Activation.IsAuthorizedUser(Values.Singleton.Username, Values.Singleton.Password))
            {
                RegisterEvents();
                UpdateLauncher();
                GenerateShortcuts();
                Changelog();
                Update();
            }
            else
            {
                MainWindow.Singleton.Main.Content = new Auth();
            }
        }

        private void GenerateShortcuts()
        {
            foreach (ShortcutFile shortcut in Shortcuts.AsList)
            {
                shortcut.Save();
            }
        }

        private void Changelog()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://dl.getmagicdm.com/CHANGELOG", Path.Combine(Values.Singleton.ConfigDirectory, "CHANGELOG"));
                client.Dispose();
            }
        }

        private void RegisterEvents()
        {
        }

        private void UpdateLauncher()
        {
            string remote_version = Values.Singleton.VersionURL;
            string RootDirectory = Values.Singleton.ApplicationRoot;
            string ConfigFolder = Values.Singleton.ConfigDirectory;
            string local_version = Values.Singleton.LocalVersionFile;
            UpdateManager.Singleton.Init(remote_version, local_version);
            string DownloadURL = UpdateManager.Singleton.GetArchiveURL(Values.Singleton.Launcher_URL_Key);
            string LaunchExe = UpdateManager.Singleton.GetExecutableName(Values.Singleton.Launcher_Executable_Key);
            status_lbl.Content = "Checking For Updates...";
            if (UpdateManager.Singleton.CheckForUpdate(Values.Singleton.Launcher_App_Key, local_version, remote_version))
            {
                string path = Path.Combine(Values.Singleton.TempDirectory, "LauncherUpdater.exe");
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(Directory.GetParent(path).FullName);
                    }

                    client.DownloadFile("http://dl.getmagicdm.com/LauncherUpdater.exe", path);
                    client.Dispose();
                }
                new Process() { StartInfo = new ProcessStartInfo() { FileName = $"\"{path}\"", Arguments = $"\"{Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName}\" \"{DownloadURL}\" \"{LaunchExe}\"", CreateNoWindow = false } }.Start();

                UpdateManager.Singleton.UpdateVersionFile(Values.Singleton.Launcher_App_Key);
                UpdateManager.Singleton.UpdateVersionFile(Values.Singleton.Launcher_Executable_Key);
                GenerateShortcuts();
                Environment.Exit(0);
            }
        }

        private void Update()
        {
            string remote_version = Values.Singleton.VersionURL;
            string RootDirectory = Values.Singleton.ApplicationRoot;
            string ConfigFolder = Values.Singleton.ConfigDirectory;
            string ApplicationDirectory = Values.Singleton.ApplicationDirectory;
            string local_version = Values.Singleton.LocalVersionFile;
            UpdateManager.Singleton.Init(remote_version, local_version);
            string DownloadURL = UpdateManager.Singleton.GetArchiveURL(Values.Singleton.Application_URL_Key);
            string LaunchExe = UpdateManager.Singleton.GetExecutableName(Values.Singleton.Application_Executable_Key);
            status_lbl.Content = "Checking For Updates...";
            Task.Run(() =>
            {
                Updater update = Updater.Init(DownloadURL, Path.Combine(Values.Singleton.TempDirectory, "Update"), ApplicationDirectory, Path.Combine(ApplicationDirectory, LaunchExe), true);
                if (UpdateManager.Singleton.CheckForUpdate(Values.Singleton.Application_App_Key, local_version, remote_version))
                {
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Update Found...";
                    }), DispatcherPriority.ContextIdle);

                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Dowloading Update...";
                    }), DispatcherPriority.ContextIdle);

                    update.Download();
                    update.DownloadClient.DownloadProgressChanged += ( (object sender, System.Net.DownloadProgressChangedEventArgs e) => { dis.Invoke(new Action(() => { status_lbl.Content = $"Downloading Update: {e.ProgressPercentage}%"; }), DispatcherPriority.ContextIdle); } );
                    update.DownloadClient.DownloadFileCompleted += ( (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                    {
                    } );
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Installing Update...";
                    }), DispatcherPriority.ContextIdle);

                    update.Unzip();

                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Finishing Update...";
                    }), DispatcherPriority.ContextIdle);

                    update.CleanUp();
                    UpdateManager.Singleton.UpdateVersionFile(Values.Singleton.Application_App_Key);
                    UpdateManager.Singleton.UpdateVersionFile(Values.Singleton.Application_Executable_Key);
                    UpdateManager.Singleton.GetChangeLog(Values.Singleton.ConfigDirectory);
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Launching Application...";
                    }), DispatcherPriority.ContextIdle);
                    GenerateShortcuts();
                    System.Threading.Thread.Sleep(2000);
                    update.LaunchExecutable();

                }
                else
                {
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Application Up to Date...";
                    }), DispatcherPriority.ContextIdle);
                    System.Threading.Thread.Sleep(2000);
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Launching Application...";
                    }), DispatcherPriority.ContextIdle);
                    System.Threading.Thread.Sleep(2000);
                    update.LaunchExecutable();
                }

            });
        }

    }
}
