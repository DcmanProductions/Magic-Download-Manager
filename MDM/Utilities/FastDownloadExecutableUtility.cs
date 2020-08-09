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
        private readonly Dispatcher dis = Values.Singleton.MainDispatcher;
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public static string GenerateExecutable()
        {
            log.Info("Generating Download Executable");
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, Resources.fdl);
                File.SetAttributes(path, FileAttributes.Hidden);
            }

            return path;
        }


        public static void DestroyExecutable()
        {
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {

                }
            }
        }

        public async Task<Process> ExecuteAsync(DownloadFile file)
        {
            return await Task.Run(() => Execute(file));
        }

        private Process Execute(DownloadFile file)
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
            if (string.IsNullOrWhiteSpace(uri))
            {
                dis.Invoke(new Action(() =>
                {
                    Values.Singleton.CurrentFileDownloading.IsDownloading = false;
                    Values.Singleton.CurrentFileDownloading.ProgressBar.Value = 100;
                    Values.Singleton.CompletedDownloads.Add(Values.Singleton.CurrentFileDownloading);
                    Values.Singleton.DownloadQueue.Remove(Values.Singleton.CurrentFileDownloading);
                    UIUtility.RemoveDownloads(file);
                    if (Values.Singleton.DownloadQueue.Count > 0)
                    {
                        Values.Singleton.DownloadQueue[0].IsDownloading = true;
                    }
                    else
                    {
                        Values.Singleton.CurrentFileDownloading = null;
                    }
                }), DispatcherPriority.ContextIdle);
                return null;

            }

            log.Info("Running Download Executable", $"Application Arguments = -s{split} -x{conn} -d{dir} {uri} --file-allocation={( pre ? "prealloc" : "none" )}");

            string exe = GenerateExecutable();
            Process pro = new Process
            {
                StartInfo = new ProcessStartInfo() { FileName = exe, Arguments = $"-s{split} -x{conn} -d{dir} {uri} --file-allocation={( pre ? "prealloc" : "none" )}", CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }
            };

            pro.Start();

            dis.Invoke(new Action(() =>
            {
                Values.Singleton.CurrentFileDownloading.DownloadFileProcess = pro;
            }), DispatcherPriority.Normal);

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
                        Values.Singleton.CurrentFileDownloading.ProgressBar.Value = 100;
                        Values.Singleton.CompletedDownloads.Add(Values.Singleton.CurrentFileDownloading);
                        Values.Singleton.DownloadQueue.Remove(Values.Singleton.CurrentFileDownloading);
                        Values.Singleton.CurrentFileDownloading.DownloadInformation.Text = "Completed";
                        UIUtility.CompleteDownload(Values.Singleton.CurrentFileDownloading);
                        if (Values.Singleton.DownloadQueue.Count > 0)
                        {
                            Values.Singleton.DownloadQueue[0].IsDownloading = true;
                        }
                        else
                        {
                            Values.Singleton.CurrentFileDownloading = null;
                        }
                    }), DispatcherPriority.ContextIdle);
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string currentSize = "N/A", fullSize = "N/A", speed = "N/A", eta = "N/A";
                double percent = 0;

                /*
                 * 0 = [#1d9d98
                 * 1 = 14MiB
                 * 2 = /
                 * 3 = 2.5GiB
                 * 4 = 0
                 * 5 = CN:
                 * 6 = DL:
                 * 7 = ETA:2m25s]
                 */
                string[] v = line.Replace("(", " ").Replace(")", "").Replace("/", " / ").Split(' ');
                if (v.Length >= 6)
                {
                    try
                    {
                        currentSize = v[1].Replace("MiB", "Mb").Replace("GiB", "Gb");
                        fullSize = v[3].Replace("MiB", "Mb").Replace("GiB", "Gb");
                        double.TryParse(DataUtility.GetNumbers(v[4]), out percent);
                        speed = v[6].Replace("DL:", "").Replace("MiB", "mb/s").Replace("GiB", "gb/s");
                        eta = v[7].Replace("ETA:", "").Replace("]", "");
                        dis.Invoke(new Action(() =>
                        {
                            if (Values.Singleton.CurrentFileDownloading != null && Values.Singleton.CurrentFileDownloading.ProgressBar != null && Values.Singleton.CurrentFileDownloading.DownloadInformation != null)
                            {
                                if (percent < 100)
                                {
                                    Values.Singleton.CurrentFileDownloading.ProgressBar.Value = percent;
                                    Values.Singleton.CurrentFileDownloading.DownloadInformation.Text = $"{percent}% ({currentSize} / {fullSize}) ETA: {eta} Speed: {speed}";
                                }
                                else
                                {
                                    Values.Singleton.CurrentFileDownloading.ProgressBar.Value = 0;
                                    Values.Singleton.CurrentFileDownloading.DownloadInformation.Text = "";
                                }
                            }
                        }), System.Windows.Threading.DispatcherPriority.Normal);
                        log.Debug($"{percent}% ({currentSize} / {fullSize}) ETA: {eta} Speed: {speed}");
                    }
                    catch
                    {

                    }
                }
            }
            return pro;
        }

    }
}
