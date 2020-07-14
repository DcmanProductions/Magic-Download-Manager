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
        }

        private void RegiserEvents()
        {
            CopyrightBtn.Click += ( (object sender, RoutedEventArgs e) => new Process() { StartInfo = new ProcessStartInfo() { FileName = "https://drewchaseproject.com" } }.Start() );
        }
    }
}
