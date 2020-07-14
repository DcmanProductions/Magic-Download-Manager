using com.drewchaseproject.MDM.Library.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class FileUtilities
    {
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public static string OpenFolder(string path, string description)
        {
            log.Debug("Opening Select Folder Dialog");
            path = path == "" ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : path;
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                folder.SelectedPath = path;
                folder.Description = description;
                folder.ShowNewFolderButton = true;
                DialogResult result = folder.ShowDialog();
                if (result == DialogResult.OK)
                {
                    log.Info($"Setting Path from {path} to {folder.SelectedPath}");
                    return folder.SelectedPath;
                }
                else
                {
                    log.Info($"Path Remains {path}");
                    return path;
                }
            }
        }

        public static void ClearLogs()
        {
            foreach (string file in Directory.GetFiles(Values.Singleton.LogLocation, "*.log", SearchOption.TopDirectoryOnly))
            {
                if (new FileInfo(file).Name.Contains("latest.log"))
                {
                    continue;
                }

                File.Delete(file);
            }
        }
        public static void ClearConfig()
        {
            if (File.Exists(Values.Singleton.ConfigFile))
            {
                File.Delete(Values.Singleton.ConfigFile);
            }
        }
    }
}
