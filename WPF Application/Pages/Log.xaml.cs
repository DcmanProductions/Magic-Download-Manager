using ChaseLabs.CLLogger;
using com.drewchaseproject.MDM.Library.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Log.xaml
    /// </summary>
    public partial class Log : Page
    {
        private readonly LogManger log = LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(Lists.LogTypes.All);
        private readonly Dispatcher dis = Dispatcher.CurrentDispatcher;
        public Log()
        {
            InitializeComponent();
            Values.Singleton.ConsoleLogBlock = LogBlock;
            UpdateLog();
            Update();
        }


        private void Update()
        {
            double seconds = 2;
            long currentTime = DateTime.Now.Ticks, neededTime = 0;
            neededTime = DateTime.Now.AddSeconds(seconds).Ticks;
            Task.Run(() =>
            {
                while (true)
                {
                    currentTime = DateTime.Now.Ticks;
                    if (currentTime >= neededTime)
                    {
                        dis.Invoke(new Action(() =>
                        {
                            UpdateLog();
                        }), DispatcherPriority.ContextIdle);
                        neededTime = DateTime.Now.AddSeconds(seconds).Ticks;
                    }
                }
            });
        }

        private void UpdateLog()
        {
            try
            {
                if (File.Exists(Values.Singleton.LogFileLocation))
                {
                    using (StreamReader reader = new StreamReader(Values.Singleton.LogFileLocation))
                    {
                        LogBlock.Text = reader.ReadToEnd();
                        LogViewer.ScrollToBottom();
                    }
                }
            }
            catch
            {
            }
        }

    }
}
