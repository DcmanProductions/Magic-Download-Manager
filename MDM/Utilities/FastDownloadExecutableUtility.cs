using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public class FastDownloadExecutableUtility
    {
        Dispatcher dis = Values.Singleton.MainDispatcher;
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public static string GenerateExecutable()
        {
            log.Info("Generating Download Executable");
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, Resources.fdl);
            File.SetAttributes(path, FileAttributes.Hidden);
            return path;
        }


        public static void DestroyExecutable()
        {
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task<Process> ExecuteAsync(DownloadFile file)
        {
            return await Task.Run(() => Execute(file));
        }

        Process Execute(DownloadFile file)
        {
            int split = 1, conn = 1;
            string dir = @"", uri = "";
            bool pre = false;

            dis.Invoke(new Action(() =>
            {
                split = Values.Singleton.CurrentFileDownloading.MaxSplitSize;
                conn = Values.Singleton.CurrentFileDownloading.Proxys;
                dir = Values.Singleton.DownloadDirectory;
                uri = Values.Singleton.DirectDownloadURI;
                pre = Values.Singleton.CurrentFileDownloading.PreAllocate;
            }), DispatcherPriority.ContextIdle);
            log.Info("Running Download Executable", $"Application Arguments = -s{split} -x{conn} -d{dir} {uri} --file-allocation={( pre ? "prealloc" : "none" )}");

            string exe = GenerateExecutable();

            Process pro = new Process
            {
                StartInfo = new ProcessStartInfo() { FileName = exe, Arguments = $"-s{split} -x{conn} -d{dir} {uri} --file-allocation={( pre ? "prealloc" : "none" )}", CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }
            };
            pro.Start();
            pro.Exited += (sx, ex) =>
            {
            };
            while (!pro.StandardOutput.EndOfStream)
            {
                string line = pro.StandardOutput.ReadLine();
                if (line.Contains("download completed"))
                {

                    dis.Invoke(new Action(() =>
                    {
                        Values.Singleton.CurrentFileDownloading.IsDownloading = false;
                        Values.Singleton.DownloadQueue.Remove(Values.Singleton.CurrentFileDownloading);
                        Values.Singleton.CurrentFileDownloading.ProgressBar.Value = 100;
                        UIUtility.GenerateDownloadUI(Values.Singleton.DownloadView);
                    }), DispatcherPriority.ContextIdle);
                }
                if(!string.IsNullOrWhiteSpace(line))
                log.Debug(line);
                string percent = "";
                bool isIn = false;
                foreach (char c in line.ToCharArray())
                {
                    if (isIn && c != '(' && c != ')')
                    {
                        percent += c;
                    }

                    if (c == '(')
                    {
                        isIn = true;
                    }

                    if (c == ')')
                    {
                        isIn = false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(percent) && int.TryParse(percent.Replace("%", ""), out int i))
                {
                    dis.Invoke(new Action(() =>
                    {
                        Values.Singleton.CurrentFileDownloading.CurrentProgress = i;
                        Values.Singleton.CurrentFileDownloading.ProgressBar.Value = i;
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                }

            }
            return pro;
        }

    }
}
