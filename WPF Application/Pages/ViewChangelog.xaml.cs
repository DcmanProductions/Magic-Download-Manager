using com.drewchaseproject.MDM.Library.Data;
using System.Windows.Controls;

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
