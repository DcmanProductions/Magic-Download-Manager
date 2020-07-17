using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using System.Windows.Controls;

namespace com.drewchaseproject.MDM.WPF.Pages.Settings_Sections
{
    /// <summary>
    /// Interaction logic for AccountSettingsSection.xaml
    /// </summary>
    public partial class AccountSettingsSection : Page
    {
        public AccountSettingsSection()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
            if (Values.Singleton.Activated)
            {
                LoginSection.Visibility = System.Windows.Visibility.Collapsed;
                LoggedInSection.Visibility = System.Windows.Visibility.Visible;
                LoginText.Content = $"Welcome {Values.Singleton.Username}";
            }
            else
            {
                LoginSection.Visibility = System.Windows.Visibility.Visible;
                LoggedInSection.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void RegisterEvents()
        {
            ActivateAccountButton.Click += (s, e) =>
            {
                bool act = Activation.IsAuthorizedUser(EmailTxtBx.Text, PasswdTxtBx.Password);
                if (act)
                {
                    Values.Singleton.Activated = true;
                    Values.Singleton.Username = EmailTxtBx.Text;
                    Values.Singleton.Password = PasswdTxtBx.Password;
                    MainWindow.Singleton.ChangeView(MainWindow.PageType.Settings);
                    MainWindow.Singleton.MenuBar.Visibility = System.Windows.Visibility.Visible;
                }
            };
        }
    }
}
