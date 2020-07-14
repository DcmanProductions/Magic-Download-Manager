using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System.Diagnostics;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.Library.Objects
{
    public class DownloadFile
    {
        public string URL { get; set; }
        public string FileName => URL.Split('/')[URL.Split('/').Length - 1].Replace("/", "");
        public double CurrentProgress { get; set; }

        public string DownloadLocation { get; set; }

        private int split;
        public int MaxSplitSize
        {
            get
            {

                if (split < 1)
                {
                    split = 1;
                    return 1;
                }
                return split;
            }
            set
            {
                if (value < 1)
                {
                    split = 1;
                }
                else
                {
                    split = value;
                }
            }
        }

        private int proxys;
        public int Proxys
        {
            get
            {
                if (proxys > 16)
                {
                    proxys = 16;
                    return 16;
                }
                else if (proxys < 1)
                {
                    proxys = 1;
                    return 1;
                }
                return proxys;
            }
            set
            {
                if (value > 16)
                {
                    proxys = 16;
                }
                else if (value < 1)
                {
                    proxys = 1;
                }
                else
                {
                    proxys = value;
                }
            }
        }

        public bool PreAllocate { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public Process DownloadFileProcess { get; set; }

        private bool _isdownloading = false;
        public bool IsDownloading
        {
            get => _isdownloading;
            set
            {
                if (value)
                {
                    Values.Singleton.CurrentFileDownloading = this;
                    Values.Singleton.DirectDownloadURI = URL;
                    LoadProcess();
                }
                _isdownloading = value;
            }
        }

        async void LoadProcess()
        {
            DownloadFileProcess = await new FastDownloadExecutableUtility().ExecuteAsync(this);
        }

    }
}
