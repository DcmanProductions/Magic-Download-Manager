using System;
using System.Net;

namespace com.drewchaseproject.MDM.Library.Data.DB
{
    public class Activation
    {

        public static bool IsAuthorizedUser(string username, string password)
        {
            if (username == "" || password == "")
            {
                return false;
            }

            try
            {
                string url = $"https://auth.drewchaseproject.com/mdm.php?username={username}&password={password}";
                using (WebClient client = new WebClient())
                {
                    string text = client.DownloadString(url);
                    Console.WriteLine(text);
                    if (text.ToLower().Equals("NOT AUTHORIZED".ToLower()) || text.ToLower().Equals("Either The Username or Password Were not valid".ToLower()))
                    {
                        return false;
                    }
                    else if (text.ToLower().Equals("Authorized".ToLower()))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

    }
}
