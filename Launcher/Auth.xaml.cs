using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using System.Diagnostics;
using System.Windows.Controls;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
            Setup();
            RegisterEvents();
        }


        private void Setup()
        {
        }

        private void RegisterEvents()
        {
            openAccountURL.Click += (s, e) => new Process() { StartInfo = new ProcessStartInfo() { FileName = "https://getmagicdm.com/index.php#header15-s" } }.Start();

            ActivateAccountButton.Click += (s, e) =>
            {
                bool act = Activation.IsAuthorizedUser(EmailTxtBx.Text, PasswdTxtBx.Password);
                if (act)
                {
                    Values.Singleton.Activated = true;
                    Values.Singleton.Username = EmailTxtBx.Text;
                    Values.Singleton.Password = PasswdTxtBx.Password;
                    MainWindow.Singleton.Main.Content = new Launching();
                }
                else
                {
                    EmailTxtBx.Text = "";
                    PasswdTxtBx.Password = "";
                }
            };
        }
    }
}
