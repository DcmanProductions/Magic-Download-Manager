using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Objects;
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

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Downloads.xaml
    /// </summary>
    public partial class Downloads : Page
    {
        public Downloads()
        {
            InitializeComponent();
        }

        void RegisterEvents()
        {
            AddDownloadBtn.Click += ((object sender, RoutedEventArgs e) =>
            {
                GenerateDownloadUI(new DownloadFile()
                {
                    URL = "https://mirrors.lug.mtu.edu/ubuntu-releases/20.04/ubuntu-20.04-desktop-amd64.iso",
                    CurrentProgress = new Random().Next(0, 100)
                });
            });
        }


        void GenerateDownloadUI(params DownloadFile[] files)
        {
            if (files.Length > 0)
            {
                Values.Singleton.DownloadQueue.AddRange(files);
            }
            foreach (var file in Values.Singleton.DownloadQueue)
            {
                var dock = new DockPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                var stack = new StackPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                var title = new TextBlock()
                {
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Text = file.FileName,
                    FontSize = 24
                };

                var border = new Border()
                {
                    BorderThickness = new Thickness(10),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(12, 9, 38)),
                    Background = new SolidColorBrush(Color.FromRgb(12, 9, 38)),
                    CornerRadius = new CornerRadius(5)
                };
                border.Child = new ProgressBar()
                {
                    Value = file.CurrentProgress
                };
                stack.Children.Add(title);
                stack.Children.Add(border);
                dock.Children.Add(stack);
                DownloadViewer.Children.Add(dock);
            }
            Values.Singleton.DownloadQueue[0].IsDownloading = true;
        }
    }
}
