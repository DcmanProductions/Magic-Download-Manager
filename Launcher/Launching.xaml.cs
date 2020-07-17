using ChaseLabs.CLUpdate;
using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using System;
using System.Diagnostics;
using System.IO;
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
                Update();
            }
            else
            {
                MainWindow.Singleton.Main.Content = new Auth();
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
            string url_key = "LAUNCHER_URL";
            string exe_key = "LAUNCHER_EXE";
            string app_key = "LAUNCHER";
            UpdateManager.Singleton.Init(remote_version, local_version);
            string DownloadURL = UpdateManager.Singleton.GetArchiveURL(url_key);
            string LaunchExe = UpdateManager.Singleton.GetExecutableName(exe_key);
            status_lbl.Content = "Checking For Updates...";
            Console.WriteLine(Path.Combine(RootDirectory, "Update"));
            if (UpdateManager.Singleton.CheckForUpdate(app_key, local_version, remote_version))
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

                    client.DownloadFile("https://www.dropbox.com/s/mjhgowibs1vcd3y/LauncherUpdater.exe?dl=1", path);
                    client.Dispose();
                }
                Console.WriteLine($"\"{path}\" \"{Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName}\" \"{DownloadURL}\" \"{LaunchExe}\"");
                new Process() { StartInfo = new ProcessStartInfo() { FileName = $"\"{path}\"", Arguments = $"\"{Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName}\" \"{DownloadURL}\" \"{LaunchExe}\"", CreateNoWindow = false } }.Start();

                UpdateManager.Singleton.UpdateVersionFile(app_key);
                UpdateManager.Singleton.UpdateVersionFile(exe_key);
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
            string url_key = "URL";
            string exe_key = "EXE";
            string app_key = "Application";
            UpdateManager.Singleton.Init(remote_version, local_version);
            string DownloadURL = UpdateManager.Singleton.GetArchiveURL(url_key);
            string LaunchExe = UpdateManager.Singleton.GetExecutableName(exe_key);
            status_lbl.Content = "Checking For Updates...";
            Task.Run(() =>
            {
                Updater update = Updater.Init(DownloadURL, Path.Combine(Values.Singleton.TempDirectory, "Update"), ApplicationDirectory, Path.Combine(ApplicationDirectory, LaunchExe), true);
                if (UpdateManager.Singleton.CheckForUpdate(app_key, local_version, remote_version))
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
                    UpdateManager.Singleton.UpdateVersionFile(app_key);
                    UpdateManager.Singleton.UpdateVersionFile(exe_key);
                    dis.Invoke(new Action(() =>
                    {
                        status_lbl.Content = "Launching Application...";
                    }), DispatcherPriority.ContextIdle);
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
