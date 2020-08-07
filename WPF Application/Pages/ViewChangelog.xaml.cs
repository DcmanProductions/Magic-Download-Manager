using com.drewchaseproject.MDM.Library.Data;
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
    /// Interaction logic for ViewChangelog.xaml
    /// </summary>
    public partial class ViewChangelog : Page
    {
        public ViewChangelog()
        {
            InitializeComponent();
            ChangeLogViewer.Text = Values.Singleton.ChangeLog;
        }
    }
}
