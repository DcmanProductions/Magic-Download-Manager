using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.drewchaseproject.MDM.Library.Objects
{
    public class DownloadFile
    {
        public string URL { get; set; }
        public string FileName => URL.Split('/')[URL.Split('/').Length - 1].Replace("/", "");
        public double CurrentProgress { get; set; }

        bool _isdownloading = false;
        public bool IsDownloading
        {
            get
            {
                return _isdownloading;
            }
            set
            {
                if (value)
                {
                    Values.Singleton.CurrentFileDownloading = this;
                    Values.Singleton.DirectDownloadURI = URL;
                    FastDownloadExecutableUtility.GenerateExecutable();
                    FastDownloadExecutableUtility.Execute();
                }
                _isdownloading = value;
            }
        }
    }
}
