using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.drewchaseproject.MDM.WPF.Pages.Template
{
    /// <summary>
    /// Interaction logic for DownloadTemplate.xaml
    /// </summary>
    public partial class DownloadTemplate : Page
    {
        public DownloadFile downloadFile;

        public DownloadTemplate(DownloadFile file)
        {
            InitializeComponent();
            downloadFile = file;
            DownloadTitle.Text = downloadFile.FileName;
            downloadFile.ProgressBar = DownloadProgressBar;
            DownloadInformation.Text = "";
            downloadFile.DownloadInformation = DownloadInformation;

            downloadFile.ProgressBar.ValueChanged += (s, e) =>
            {
                DownloadInformation.Text = $"{downloadFile.ProgressBar.Value}%";
            };

            RemoveBtn.Click += (s, e) =>
            {
                UIUtility.RemoveDownloads(file);
                if (file.DownloadFileProcess != null)
                    if (!file.DownloadFileProcess.HasExited)
                        file.DownloadFileProcess.Kill();
                if (Values.Singleton.CurrentFileDownloading != null)
                    if (Values.Singleton.DownloadQueue.Count > 0)
                        Values.Singleton.DownloadQueue[0].IsDownloading = true;
                    else
                        Values.Singleton.CurrentFileDownloading = null;
            };

        }
    }
}
