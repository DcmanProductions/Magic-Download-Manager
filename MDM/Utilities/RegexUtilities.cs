using com.drewchaseproject.MDM.Library.Data;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class RegexUtilities
    {
        private static readonly ChaseLabs.CLLogger.LogManger log = ChaseLabs.CLLogger.LogManger.Init().SetLogDirectory(Values.Singleton.LogFileLocation).EnableDefaultConsoleLogging().SetMinLogType(ChaseLabs.CLLogger.Lists.LogTypes.All);
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Warn("Email Was Blank");
                return false;
            }
            log.Debug($"Checking if Email ({email}) is valid");

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    IdnMapping idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                log.Warn($"{email} was NOT a valid Email");
                return false;
            }
            catch (ArgumentException e)
            {
                log.Warn($"{email} was NOT a valid Email");
                return false;
            }

            try
            {
                bool b = Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (b)
                {
                    log.Info($"{email} was VALID");
                }
                else
                {
                    log.Warn($"{email} was NOT valid");
                }

                return b;
            }
            catch (RegexMatchTimeoutException)
            {
                log.Warn($"{email} was NOT a valid Email");
                return false;
            }
        }
    }
}