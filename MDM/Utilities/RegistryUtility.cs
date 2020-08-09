using com.drewchaseproject.MDM.Library.Data;
using Microsoft.Win32;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class RegistryUtility
    {
        private static readonly string run = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", protocol = @"magicdm\shell\open\command";
        public static void AddToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(run, true))
            {
                key.SetValue(Values.Singleton.ApplicationName.Replace(" ", "_"), $"\"{Values.Singleton.LauncherExe}\"");
            }
        }

        public static bool IsAddedToStartup
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(run, true))
                {
                    if (key.GetValue(Values.Singleton.ApplicationName.Replace(" ", "_")) != null)
                    {
                        if (key.GetValue(Values.Singleton.ApplicationName.Replace(" ", "_")).GetType().Equals(typeof(string)))
                        {
                            if ((string) key.GetValue(Values.Singleton.ApplicationName.Replace(" ", "_")) != $"\"{Values.Singleton.LauncherExe}\"")
                            {
                                AddToStartup();
                            }
                        }
                    }

                    return key.GetValue(Values.Singleton.ApplicationName.Replace(" ", "_")) != null;
                }
            }
        }

        public static void RemoveFromStartup()
        {
            if (IsAddedToStartup)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(run, true))
                {
                    key.DeleteValue(Values.Singleton.ApplicationName.Replace(" ", "_"));
                }
            }
        }


        public static void AddUrlProtocol()
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(protocol, true))
            {
                key.SetValue("", $"\"{Values.Singleton.LauncherExe}\" \"%1\"");
            }
        }

        public static bool IsUrlProtocolAdded
        {
            get
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(protocol, true))
                {
                    if (key.GetValue("") != null)
                    {
                        if (key.GetValue("").GetType().Equals(typeof(string)))
                        {
                            if ((string) key.GetValue("") != $"\"{Values.Singleton.LauncherExe}\" \"%1\"")
                            {
                                AddUrlProtocol();
                            }
                        }
                    }

                    return key.GetValue("") != null;
                }
            }
        }

        public static void RemoveUrlProtocol()
        {
            if (IsUrlProtocolAdded)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(protocol, true))
                {
                    key.DeleteValue("");
                }
            }
        }


    }
}
