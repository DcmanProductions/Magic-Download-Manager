using ChaseLabs.CLLogger.Events;
using System;
using System.Net;

namespace com.drewchaseproject.MDM.Library.Data.DB
{
    public class Activation
    {
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);


        public static bool IsAuthorizedUser(string username, string password)
        {
            if (username == "" || password == "")
            {
                return false;
            }

            try
            {
                string url = $"https://auth.getmagicdm.tk/check.php?username={username}&password={password}";
                using (WebClient client = new WebClient())
                {
                    string text = client.DownloadString(url);
                    log.Info(text);
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
