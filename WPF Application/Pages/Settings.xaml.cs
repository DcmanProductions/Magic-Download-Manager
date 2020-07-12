using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Utilities;
using com.drewchaseproject.MDM.WPF.Pages.Settings_Sections;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();

            LoadPages( new DownloadSettingsSection(),new AccountSettingsSection());

        }

        void LoadPages(params Page[] pages)
        {
            foreach (Page page in pages)
            {
                SettingsView.Children.Add(new Frame() { Content = page, Margin = new Thickness(0,0,0,50) });
            }
        }

    }
}
