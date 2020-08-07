using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Documents;
using System.Windows.Forms;
using File = System.IO.File;

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

        public static string OpenFile(string path, string description)
        {
            log.Debug("Opening Select File Dialog");
            path = path == "" ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : path;
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Title = description;
                file.InitialDirectory = path;
                DialogResult result = file.ShowDialog();
                if (result == DialogResult.OK)
                {
                    log.Info($"Setting Path from {path} to {file.FileName}");
                    return file.FileName;
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

                System.IO.File.Delete(file);
            }
        }
        public static void ClearConfig()
        {
            if (File.Exists(Values.Singleton.ConfigFile))
            {
                File.Delete(Values.Singleton.ConfigFile);
            }
        }
        public static bool IsSingleFile(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);

                if (( attr & FileAttributes.Directory ) == FileAttributes.Directory)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                log.Error("An Error has Occurred while Checking Valid Path", ex);
            }

            return isValid;
        }

        public static void ExportDownloads()
        {
            if (File.Exists(Values.Singleton.DownloadCache)) File.Delete(Values.Singleton.DownloadCache);
            using (var writer = new StreamWriter(Values.Singleton.DownloadCache))
            {
                foreach (var file in Values.Singleton.DownloadQueue)
                {
                    writer.WriteLine(file.URL);
                }
                writer.Flush();
                writer.Dispose();
                writer.Close();
            }
        }


        public static List<DownloadFile> ImportDownloads()
        {
            return ImportDownloads(Values.Singleton.DownloadCache);
        }

        public static List<DownloadFile> ImportDownloads(string file)
        {
            List<DownloadFile> value = new List<DownloadFile>();
            if (!File.Exists(file)) return value;
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    value.Add(new DownloadFile() { URL = line });
                }
            }
            return value;

        }


    }
}
