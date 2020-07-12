using ChaseLabs.CLConfiguration.List;
using ChaseLabs.CLConfiguration.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.drewchaseproject.MDM.Library.Data
{
    public class Configuration
    {
        static Configuration _singleton;
        public static Configuration Singleton => _singleton == null ? _singleton = new Configuration() : _singleton;

        public ConfigManager manager;

        public Configuration()
        {
            manager = new ConfigManager(Path.Combine(Values.Singleton.ConfigDirectory, "settings.cfg"));
            manager.Add(new Config("Max Connections", "16", manager));
            manager.Add(new Config("File Split Count", "100", manager));
            manager.Add(new Config("PreAllocate", "false", manager));
            manager.Add(new Config("Download Directory", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), manager));
        }
    }
}
