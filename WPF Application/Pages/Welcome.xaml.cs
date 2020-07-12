using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        void Setup()
        {
            CopyrightLbl.Content += $"-{DateTime.Now.Year}";
        }

        void RegiserEvents()
        {
            CopyrightBtn.Click += ((object sender, RoutedEventArgs e) => new Process() { StartInfo = new ProcessStartInfo() { FileName = "https://drewchaseproject.com" } }.Start());
        }
    }
}
