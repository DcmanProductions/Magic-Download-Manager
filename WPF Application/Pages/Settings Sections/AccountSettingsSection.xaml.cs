using System;
using System.Data;
using System.Data.SqlClient;
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
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection("SERVER = drewchaseproject.com; DATABASE = dbvk8n7ktfcee7; PORT = 3306; USER ID = ue6zchn3j43vw; PASSWORD = 11d_[beg1((b");
                connection.Open();
            }
            catch
            {
                Console.WriteLine("Connection String Invalid");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void RegisterEvents()
        {
            //ActivateAccountButton.Click += (s, e) => Activation.ActivateSoftware(EmailTxtBx.Text, PasswdTxtBx.Password);
        }
    }
}
