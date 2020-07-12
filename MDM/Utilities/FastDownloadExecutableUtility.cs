using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class FastDownloadExecutableUtility
    {
        public static string GenerateExecutable()
        {
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllBytes(path, Resources.fdl);
            File.SetAttributes(path, FileAttributes.Hidden);
            return path;
        }


        public static void DestroyExecutable()
        {
            string path = Path.Combine(Values.Singleton.TempDirectory, "mdm.exe");
            if (File.Exists(path)) File.Delete(path);
        }

        public static Process Execute()
        {
            string exe = GenerateExecutable();
            Process pro = new Process();
            pro.StartInfo = new ProcessStartInfo() { FileName = exe, Arguments = $"-s{Values.Singleton.FileSplitCount} -x{Values.Singleton.ConnectionsPerProxy} -d{Values.Singleton.DownloadDirectory} {Values.Singleton.DirectDownloadURI} --file-allocation={(Values.Singleton.PreAllocate ? "prealloc" : "none")}", CreateNoWindow = false };
            pro.Start();
            pro.Exited += ((object sender, EventArgs e) =>
            {
                Values.Singleton.CurrentFileDownloading.IsDownloading = false;
                DestroyExecutable();
            });
            return pro;
        }

    }
}
