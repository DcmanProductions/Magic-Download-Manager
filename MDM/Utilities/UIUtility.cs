using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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


        public static void GenerateDownloadUI(StackPanel DownloadViewer, params DownloadFile[] files)
        {
            DownloadViewer.Children.Clear();
            if (files.Length > 0)
            {
                Values.Singleton.DownloadQueue.AddRange(files);
            }
            foreach (DownloadFile file in Values.Singleton.DownloadQueue)
            {
                DockPanel dock = new DockPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                StackPanel stack = new StackPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                TextBlock title = new TextBlock()
                {
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Text = file.FileName,
                    FontSize = 24,
                    Foreground = new SolidColorBrush(Color.FromRgb(231, 52, 120))
                };

                Border border = new Border()
                {
                    BorderThickness = new Thickness(10),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(12, 9, 38)),
                    Background = new SolidColorBrush(Color.FromRgb(12, 9, 38)),
                    CornerRadius = new CornerRadius(5)
                };
                ProgressBar bar = new ProgressBar()
                {
                    Value = file.CurrentProgress
                };
                file.ProgressBar = bar;
                border.Child = bar;

                stack.Children.Add(title);
                stack.Children.Add(border);
                dock.Children.Add(stack);
                DownloadViewer.Children.Add(dock);
            }
        }


    }
}
