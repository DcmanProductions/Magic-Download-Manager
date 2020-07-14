using System;
using System.Data.SqlClient;
using System.Windows;

namespace com.drewchaseproject.MDM.Library.Data.DB
{
    public class Activation
    {
        private const string connection_string = "SERVER = giow1059.siteground.us; DATABASE = dbvk8n7ktfcee7; USER ID = ue6zchn3j43vw; PASSWORD = 11d_[beg1((b";
        private static bool Activated { get; set; }

        public static bool isActivated(string username, string password)
        {
            using (SqlConnection sqlConn = new SqlConnection(connection_string))
            {
                string checkForActivationQuery = "SELECT activated FROM ActivationTable WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(checkForActivationQuery, sqlConn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                sqlConn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public static void ActivateSoftware(string username, string password)
        {
            if (!isActivated(username, password))
            {
                using (SqlConnection sqlConn = new SqlConnection(connection_string))
                {
                    string checkForKeyQuery = "SELECT COUNT(*) FROM ActivationTable WHERE username = @username AND password = @password";
                    SqlCommand cmd = new SqlCommand(checkForKeyQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    sqlConn.Open();
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    if (result > 0)
                    {
                        updateActivation(username, password);
                        Activated = true;
                    }
                    else
                    {
                        MessageBox.Show("Your Key Was Incorrect");
                        Console.WriteLine("Your Key Was Incorrect");
                        Activated = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Software is Already Activated");
                Console.WriteLine("Your Software is Already Activated");
                Activated = true;
            }
        }

        private static void updateActivation(string username, string password)
        {
            using (SqlConnection sqlConn = new SqlConnection(connection_string))
            {
                string updateQuery = "UPDATE ActivationTable SET activated = 1 WHERE WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(updateQuery, sqlConn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                sqlConn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your Software has Been Activated");
                Console.WriteLine("Your Software has Been Activated");
            }
        }

    }
}
