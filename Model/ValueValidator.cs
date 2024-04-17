using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FingerPrintManagerApp.Model
{
    public class ValueValidator
    {
        public static bool IsValidPhoneNumber(string telephone)
        {
            string pattern = "(8(0|1|2|4|5|9)|9(0|7|8|9))[0-9]{7}";
            return Regex.IsMatch(telephone, pattern);
        }

        public static bool IsStrongPassword(string passwd)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,32}$";
            return Regex.IsMatch(passwd, pattern);
        }

        static bool invalid = false;
        public static bool IsValidEmail(string email)
        {
            invalid = false;
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidISOCode3(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            var pattern = "[a-zA-Z]{3,3}";
            return Regex.IsMatch(code, pattern);
        }

        public static bool IsValidTelephone(string telephone)
        {
            if (string.IsNullOrWhiteSpace(telephone))
                return false;

            var pattern = "[0-9]{9,15}";
            return Regex.IsMatch(telephone, pattern);
        }

        public static bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            var pattern = "(ISBN[-]*(1[03])*[]*(: ){0,1})*(([0-9Xx][- ]*){13}|([0-9Xx][- ]*{10}))";
            return Regex.IsMatch(isbn, pattern);
        }

        public static bool IsValidAccountNumber(string account)
        {
            var pattern = "[0-9]{9,20}";
            return Regex.IsMatch(account, pattern);
        }

        public static bool IsValidRFID(string rfid)
        {
            var pattern = "[0-9]{9,20}";
            return Regex.IsMatch(rfid, pattern);
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static bool IsValidURL(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }
    }
}
