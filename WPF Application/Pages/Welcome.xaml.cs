using com.drewchaseproject.MDM.Library.Data;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        public Welcome()
        {
            InitializeComponent();
            Setup();
            RegiserEvents();
        }

        private void Setup()
        {
            CopyrightLbl.Content += $"-{DateTime.Now.Year}";
            VersionsLbl.Content = $"app.{Values.Singleton.ApplicationVersion} | launcher.{Values.Singleton.LauncherVersion}";
        }

        private void RegiserEvents()
        {
            OpenChangelogBtn.Click += (s, e) => MainWindow.Singleton.Main.Content = new ViewChangelog();
            CopyrightBtn.Click += ( (object sender, RoutedEventArgs e) => new Process() { StartInfo = new ProcessStartInfo() { FileName = "https://drewchaseproject.com" } }.Start() );
        }
    }
}
