using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Net.Mime;
using System.Windows;

namespace com.drewchaseproject.MDM.Library.Objects
{
    public class DownloadFile
    {
        public string URL { get; set; }
        public string FileName
        {
            get
            {

                string filename = "";

                if (string.IsNullOrEmpty(filename))
                {
                    string s = URL.Split('/')[URL.Split('/').Length - 1].Replace("/", "");
                    char[] local = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~`‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ".ToCharArray();
                    string[] reference = new string[] { "%20", "%21", "%22", "%23", "%24", "%25", "%26", "%27", "%28", "%29", "%2A", "%2B", "%2C", "%2D", "%2E", "%2F", "%30", "%31", "%32", "%33", "%34", "%35", "%36", "%37", "%38", "%39", "%3A", "%3B", "%3C", "%3D", "%3E", "%3F", "%40", "%41", "%42", "%43", "%44", "%45", "%46", "%47", "%48", "%49", "%4A", "%4B", "%4C", "%4D", "%4E", "%4F", "%50", "%51", "%52", "%53", "%54", "%55", "%56", "%57", "%58", "%59", "%5A", "%5B", "%5C", "%5D", "%5E", "%5F", "%60", "%61", "%62", "%63", "%64", "%65", "%66", "%67", "%68", "%69", "%6A", "%6B", "%6C", "%6D", "%6E", "%6F", "%70", "%71", "%72", "%73", "%74", "%75", "%76", "%77", "%78", "%79", "%7A", "%7B", "%7C", "%7D", "%7E", "%80", "%81", "%82", "%83", "%84", "%85", "%86", "%87", "%88", "%89", "%8A", "%8B", "%8C", "%8D", "%8E", "%8F", "%90", "%91", "%92", "%93", "%94", "%95", "%96", "%97", "%98", "%99", "%9A", "%9B", "%9C", "%9D", "%9E", "%9F", "%A1", "%A2", "%A3", "%A4", "%A5", "%A6", "%A7", "%A8", "%A9", "%AA", "%AB", "%AC", "%AE", "%AF", "%B0", "%B1", "%B2", "%B3", "%B4", "%B5", "%B6", "%B7", "%B8", "%B9", "%BA", "%BB", "%BC", "%BD", "%BE", "%BF", "%C0", "%C1", "%C2", "%C3", "%C4", "%C5", "%C6", "%C7", "%C8", "%C9", "%CA", "%CB", "%CC", "%CD", "%CE", "%CF", "%D0", "%D1", "%D2", "%D3", "%D4", "%D5", "%D6", "%D7", "%D8", "%D9", "%DA", "%DB", "%DC", "%DD", "%DE", "%DF", "%E0", "%E1", "%E2", "%E3", "%E4", "%E5", "%E6", "%E7", "%E8", "%E9", "%EA", "%EB", "%EC", "%ED", "%EE", "%EF", "%F0", "%F1", "%F2", "%F3", "%F4", "%F5", "%F6", "%F7", "%F8", "%F9", "%FA", "%FB", "%FC", "%FD", "%FE", "%FF" };
                    for (int i = 0; i < reference.Length; i++)
                    {
                        s = s.Replace(reference[i], "" + local[i]);
                    }

                    filename = s.Split('?')[0].Replace("?", "");
                }

                return filename;
            }
        }

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
        public TextBlock DownloadInformation { get; set; }
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

        private async void LoadProcess()
        {
            DownloadFileProcess = await new FastDownloadExecutableUtility().ExecuteAsync(this);
        }

    }
}
