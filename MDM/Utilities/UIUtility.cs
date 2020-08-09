using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class UIUtility
    {
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public static bool ToggleCheckBox(Button button)
        {
            DockPanel dock = button.Content.GetType().Equals(typeof(DockPanel)) ? (DockPanel) button.Content : null;
            Image image = null;
            TextBlock text = null;
            if (dock != null)
            {
                foreach (object element in dock.Children)
                {
                    if (element.GetType().Equals(typeof(Image)))
                    {
                        image = (Image) element;
                    }
                    else if (element.GetType().Equals(typeof(TextBlock)))
                    {
                        text = (TextBlock) element;
                    }
                }
            }
            bool value = image.Source == null ? true : false;
            log.Debug($"Toggling Custom Check Box ({button.Name})", $"{button.Name} is {( value ? "Enabled" : "Disabled" )}");
            image.Source = value ? new BitmapImage(new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/Transparent/Check Mark.png")) : null;
            return value;
        }


        public static void RemoveDownloads(DownloadFile file)
        {

            if (Values.Singleton.DownloadQueue.Contains(file))
            {
                Values.Singleton.DownloadQueue.Remove(file);
            }
            else if (Values.Singleton.CompletedDownloads.Contains(file))
            {
                Values.Singleton.CompletedDownloads.Remove(file);
            }

            try
            {

                foreach (object element in Values.Singleton.DownloadViewer.Children)
                {
                    if (element.GetType().Equals(typeof(Frame)))
                    {
                        Frame frame = (Frame) element;
                        if (frame.Name.Equals(file.ComponentName))
                        {
                            Values.Singleton.DownloadViewer.Children.Remove(frame);
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {

            }
            catch
            {

            }
        }

        public static void CompleteDownload(DownloadFile file)
        {
            if (Values.Singleton.DownloadQueue.Contains(file))
            {
                Values.Singleton.DownloadQueue.Remove(file);
            }

            if (Values.Singleton.DownloadQueue.Count > 0)
            {
                Values.Singleton.DownloadPageTitle.Content = $"{Values.Singleton.DownloadQueue.Count} Remaining";
            }
            else
            {
                Values.Singleton.DownloadPageTitle.Content = "Downloads";
            }

            try
            {
                foreach (object element in Values.Singleton.DownloadViewer.Children)
                {
                    if (element.GetType().Equals(typeof(Frame)))
                    {
                        Frame frame = (Frame) element;
                        if (frame.Name.Equals(DataUtility.GetValidComponentName(file.FileName)))
                        {
                            Values.Singleton.DownloadViewer.Children.Remove(frame);
                            Values.Singleton.DownloadViewer.Children.Add(frame);
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {

            }
            catch
            {

            }
        }

        public static void SetCheckBox(Button button, bool value)
        {
            DockPanel dock = button.Content.GetType().Equals(typeof(DockPanel)) ? (DockPanel) button.Content : null;
            Image image = null;
            TextBlock text = null;
            if (dock != null)
            {
                foreach (object element in dock.Children)
                {
                    if (element.GetType().Equals(typeof(Image)))
                    {
                        image = (Image) element;
                    }
                    else if (element.GetType().Equals(typeof(TextBlock)))
                    {
                        text = (TextBlock) element;
                    }
                }
            }
            image.Source = value ? new BitmapImage(new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/Transparent/Check Mark.png")) : null;
        }

        public static bool GetCheckBox(Button button)
        {
            DockPanel dock = button.Content.GetType().Equals(typeof(DockPanel)) ? (DockPanel) button.Content : null;
            Image image = null;
            TextBlock text = null;
            if (dock != null)
            {
                foreach (object element in dock.Children)
                {
                    if (element.GetType().Equals(typeof(Image)))
                    {
                        image = (Image) element;
                    }
                    else if (element.GetType().Equals(typeof(TextBlock)))
                    {
                        text = (TextBlock) element;
                    }
                }
            }
            return image.Source == null ? true : false;
        }


        public static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr) 0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO) Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width => Math.Abs(right - left);
            public int Height => bottom - top;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty => left >= right || top >= bottom;
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!( obj is Rect )) { return false; }
                return ( this == (RECT) obj );
            }
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }

            public static bool operator ==(RECT rect1, RECT rect2) { return ( rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom ); }
            public static bool operator !=(RECT rect1, RECT rect2) { return !( rect1 == rect2 ); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);


    }
}
