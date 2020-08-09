using com.drewchaseproject.MDM.Library.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Process[] processes = Process.GetProcessesByName("Magic Download Manager");
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    string url = args[i];
                    if (url.StartsWith("magicdm://"))
                    {
                        url = url.Replace("magicdm://", "");
                    }

                    using (StreamWriter writer = new StreamWriter(Values.Singleton.HotSwapDownloadCache, true))
                    {
                        writer.WriteLine(url);
                        writer.Flush();
                        writer.Dispose();
                        writer.Close();
                    }
                }
            }

            if (processes.Length > 1)
            {
                Environment.Exit(0);
            }

            Values.Singleton.CurrentlyExecutingApplicationAssembly = Assembly.GetExecutingAssembly();
            Values.Singleton.LauncherExe = Assembly.GetExecutingAssembly().Location;

        }
    }
}
