using ChaseLabs.CLConfiguration.List;
using ChaseLabs.CLUpdate;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Threading;

namespace com.drewchaseproject.MDM.Library.Data
{
    public class Values
    {
        private static Values _singleton;
        public static Values Singleton => _singleton == null ? _singleton = new Values() : _singleton;

        public bool Activated { get; set; }

        public string Username { get => Crypto.DecryptStringAES(Configuration.Singleton.userauth.GetConfigByKey("Username").Value, Environment.MachineName); set => Configuration.Singleton.userauth.GetConfigByKey("Username").Value = Crypto.EncryptStringAES(value, Environment.MachineName); }
        public string Password { get => Crypto.DecryptStringAES(Configuration.Singleton.userauth.GetConfigByKey("Password").Value, Environment.MachineName); set => Configuration.Singleton.userauth.GetConfigByKey("Password").Value = Crypto.EncryptStringAES(value, Environment.MachineName); }

        public bool UpdateAvailable
        {
            get
            {
                UpdateManager.Singleton.Init(VersionURL, LocalVersionFile);
                Updater.Init(UpdateManager.Singleton.GetArchiveURL(Values.Singleton.Application_URL_Key), Path.Combine(Values.Singleton.TempDirectory, "Update"), ApplicationDirectory, Path.Combine(ApplicationDirectory, UpdateManager.Singleton.GetExecutableName(Values.Singleton.Application_Executable_Key)), true);
                return UpdateManager.Singleton.CheckForUpdate(Values.Singleton.Application_App_Key, LocalVersionFile, VersionURL);
            }
        }

        public string ChangeLog
        {
            get
            {
                using (StreamReader reader = new StreamReader(Path.Combine(Values.Singleton.ConfigDirectory, "CHANGELOG")))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public Button PlayDownloadBtn { get; set; }

        private DownloadFile _currentFileDownloading { get; set; }
        public DownloadFile CurrentFileDownloading
        {
            get => _currentFileDownloading;
            set
            {
                if (value == null)
                {
                    PlayDownloadBtn.IsEnabled = true;
                }
                else
                {
                    PlayDownloadBtn.IsEnabled = false;
                }

                _currentFileDownloading = value;
            }
        }

        public Dispatcher MainDispatcher { get; set; }

        private List<DownloadFile> _queue;
        public List<DownloadFile> DownloadQueue
        {
            get
            {
                if (_queue == null)
                {
                    _queue = new List<DownloadFile>();
                }

                return _queue;
            }
            set => _queue = value;
        }

        private List<DownloadFile> _doneQueue;
        public List<DownloadFile> CompletedDownloads { get { if (_doneQueue == null) { _doneQueue = new List<DownloadFile>(); } return _doneQueue; } set => _doneQueue = value; }

        public int ConnectionsPerProxy
        {
            get
            {
                if (Configuration.Singleton.manager.GetConfigByKey("Max Connections").ParseInt() > 16)
                {
                    ConnectionsPerProxy = 16;
                    return 16;
                }
                else if (Configuration.Singleton.manager.GetConfigByKey("Max Connections").ParseInt() < 1)
                {
                    ConnectionsPerProxy = 1;
                    return 1;
                }
                return Configuration.Singleton.manager.GetConfigByKey("Max Connections").ParseInt();
            }
            set
            {
                if (value > 16)
                {
                    Configuration.Singleton.manager.GetConfigByKey("Max Connections").Value = 16 + "";
                }
                else if (value < 1)
                {
                    Configuration.Singleton.manager.GetConfigByKey("Max Connections").Value = 1 + "";
                }
                else
                {
                    Configuration.Singleton.manager.GetConfigByKey("Max Connections").Value = value + "";
                }
            }
        }
        public int FileSplitCount
        {
            get
            {
                if (Configuration.Singleton.manager.GetConfigByKey("File Split Count").ParseInt() < 1)
                {
                    FileSplitCount = 1;
                    return 1;
                }
                return Configuration.Singleton.manager.GetConfigByKey("File Split Count").ParseInt();
            }
            set
            {
                if (value < 1)
                {
                    Configuration.Singleton.manager.GetConfigByKey("File Split Count").Value = 1 + "";
                }
                else
                {
                    Configuration.Singleton.manager.GetConfigByKey("File Split Count").Value = value + "";
                }
            }
        }

        public bool PreAllocate { get => Configuration.Singleton.manager.GetConfigByKey("PreAllocate").ParseBoolean(); set => Configuration.Singleton.manager.GetConfigByKey("PreAllocate").Value = value + ""; }
        public string DownloadDirectory { get => Configuration.Singleton.manager.GetConfigByKey("Download Directory").Value; set => Configuration.Singleton.manager.GetConfigByKey("Download Directory").Value = value; }

        private string _direct;
        public string DirectDownloadURI
        {
            get => _direct;
            set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    _direct = value;
                }
                else
                {
                    _direct = string.Empty;
                }
            }
        }

        public string CompanyName => "Chase Labs";
        public string ApplicationName => "Magic Download Manager";
        public string ApplicationRoot
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CompanyName, ApplicationName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public string ConfigDirectory
        {
            get
            {
                string path = Path.Combine(ApplicationRoot, "Configuration");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public string CacheDirectory
        {
            get
            {
                string path = Path.Combine(ApplicationRoot, "Cache");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public string DownloadCache
        {
            get
            {
                string path = Path.Combine(CacheDirectory, "leftovers");
                return path;
            }
        }
        public string HotSwapDownloadCache
        {
            get
            {
                string path = Path.Combine(CacheDirectory, "hotswap");
                return path;
            }
        }

        public string LocalVersionFile
        {
            get
            {
                string path = Path.Combine(ConfigDirectory, "version");
                return path;
            }
        }

        public string VersionURL => @"https://dl.getmagicdm.com/Version";

        public string ConfigFile
        {
            get
            {
                string path = Path.Combine(ConfigDirectory, "settings.cfg");
                return path;
            }
        }

        public string UserCacheFile
        {
            get
            {
                string path = Path.Combine(CacheDirectory, "usercache");
                return path;
            }
        }
        public string LogFileLocation
        {
            get
            {
                string path = Path.Combine(LogLocation, "latest.log");
                return path;
            }
        }

        public Assembly CurrentlyExecutingApplicationAssembly { get; set; }

        public string GetIconPath
        {
            get
            {
                string path = Path.Combine(Directory.GetParent(CurrentlyExecutingApplicationAssembly.Location).FullName, "Icon.ico");
                if (!File.Exists(path))
                {
                    System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(CurrentlyExecutingApplicationAssembly.Location);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        icon.Save(stream);
                        stream.Flush();
                        stream.Close();
                    }
                }
                return path;
            }
        }

        public string LogLocation
        {
            get
            {
                string path = Path.Combine(ApplicationRoot, "Logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        public string ApplicationDirectory
        {
            get
            {
                string path = Path.Combine(ApplicationRoot, "Bin");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public string TempDirectory
        {
            get
            {
                string path = Path.Combine(Environment.GetEnvironmentVariable("temp", EnvironmentVariableTarget.User), CompanyName, ApplicationName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        public TextBlock ConsoleLogBlock { get; set; }
        public StackPanel DownloadViewer { get; set; }
        public Label DownloadPageTitle { get; set; }


        public string Launcher_URL_Key => "LAUNCHER_URL";
        public string Launcher_Executable_Key => "LAUNCHER_EXE";
        public string Launcher_App_Key => "LAUNCHER";


        public string Application_URL_Key => "URL";
        public string Application_Executable_Key => "EXE";
        public string Application_App_Key => "Application";

        public bool MinimizeOnStart { get => Configuration.Singleton.manager.GetConfigByKey("Minimize ON Start").ParseBoolean(); set => Configuration.Singleton.manager.GetConfigByKey("Minimize ON Start").Value = value + ""; }
        public bool MaximizeOnDownload { get => Configuration.Singleton.manager.GetConfigByKey("Maximize on Download Start").ParseBoolean(); set => Configuration.Singleton.manager.GetConfigByKey("Maximize on Download Start").Value = value + ""; }
        public bool StartWithWindows
        {
            get => Configuration.Singleton.manager.GetConfigByKey("StartWithWindows").ParseBoolean();
            set
            {
                Configuration.Singleton.manager.GetConfigByKey("StartWithWindows").Value = value + "";
                if (value)
                {
                    RegistryUtility.AddToStartup();
                }
                else
                {
                    RegistryUtility.RemoveFromStartup();
                }
            }
        }
        public string LauncherExe { get => Configuration.Singleton.manager.GetConfigByKey("Launcher Directory").Value; set => Configuration.Singleton.manager.GetConfigByKey("Launcher Directory").Value = value; }
        public string LauncherInstallDirectory => Directory.GetParent(LauncherExe).FullName;

        public string ApplicationVersion => new ConfigManager(LocalVersionFile).GetConfigByKey(Application_App_Key).Value;
        public string LauncherVersion => new ConfigManager(LocalVersionFile).GetConfigByKey(Launcher_App_Key).Value;

    }
}
