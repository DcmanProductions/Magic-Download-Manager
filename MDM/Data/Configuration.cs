using ChaseLabs.CLConfiguration.List;
using ChaseLabs.CLConfiguration.Object;
using System;
using System.IO;

namespace com.drewchaseproject.MDM.Library.Data
{
    public class Configuration
    {
        private static Configuration _singleton;
        public static Configuration Singleton => _singleton == null ? _singleton = new Configuration() : _singleton;
        private readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);

        public ConfigManager manager;

        public Configuration()
        {
            manager = new ConfigManager(Values.Singleton.ConfigFile);
            log.Debug($"Initializing Config at {manager.PATH}");
            manager.Add(new Config("Max Connections", "16", manager));
            manager.Add(new Config("File Split Count", "100", manager));
            manager.Add(new Config("PreAllocate", "false", manager));
            manager.Add(new Config("Username", "", manager));
            manager.Add(new Config("Password", "", manager));
            manager.Add(new Config("Download Directory", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), manager));
            manager.List().ForEach((n) => log.Debug($"Loading Config for {n.Key} with Value of {n.Value}"));
        }
    }
}
