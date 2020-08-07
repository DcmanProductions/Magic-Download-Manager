using com.drewchaseproject.MDM.Library.Data;
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

namespace com.drewchaseproject.MDM.WPF.Pages.Settings_Sections
{
    /// <summary>
    /// Interaction logic for ApplicationSettingsSection.xaml
    /// </summary>
    public partial class ApplicationSettingsSection : Page
    {
        public ApplicationSettingsSection()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        void Setup()
        {
            UIUtility.SetCheckBox(StartWithWindowsCheckBtn, Values.Singleton.StartWithWindows);
            UIUtility.SetCheckBox(StartMinimizedCheckBtn, Values.Singleton.MinimizeOnStart);
            UIUtility.SetCheckBox(MaximizeOnDownloadCheckBtn, Values.Singleton.MaximizeOnDownload);
        }

        void RegisterEvents()
        {
            StartWithWindowsCheckBtn.Click += (s, e) => Values.Singleton.StartWithWindows = UIUtility.ToggleCheckBox((Button) s);
            StartMinimizedCheckBtn.Click += (s, e) => Values.Singleton.MinimizeOnStart = UIUtility.ToggleCheckBox((Button) s);
            MaximizeOnDownloadCheckBtn.Click += (s, e) => Values.Singleton.MaximizeOnDownload = UIUtility.ToggleCheckBox((Button) s);
        }

    }
}
