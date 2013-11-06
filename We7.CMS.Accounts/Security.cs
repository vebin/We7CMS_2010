using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Web.Security;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    public class Security
    {
        public static event Action<string> AfterSetAccountID;

        static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        public static string CurrentAccountID
        {
            get
            {
                string currentAccountID = string.Empty;
                if (Context != null)
                {
                    if (Context.Request != null && Context.Request.IsAuthenticated)
                    {
                        try
                        {
                            currentAccountID = Context.User.Identity.Name;
                        }
                        catch
                        {
                        }
                    }
                    else if (Context.Session != null && Context.Session[AccountLocalHelper.AccountSessionKey] != null)
                    {
                        currentAccountID = Context.Session[AccountLocalHelper.AccountSessionKey] as string;
                    }
                }

                return currentAccountID;
            }
        }

        public static bool IsAuthenticated()
        {

            return !string.IsNullOrEmpty(CurrentAccountID);
        }

        public static string Encrypt(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            else
            {
                password = password.ToLower();
                byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
                byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                return BitConverter.ToString(hashedBytes);
            }
        }

        public static void SetAccountID(string accountID)
        {
            SetTicket(accountID, GeneralConfigs.GetConfig().EnableCookieAuthentication);
            Context.Session[AccountLocalHelper.AccountSessionKey] = accountID;
            if (AfterSetAccountID != null)
            {
                AfterSetAccountID(accountID);
            }
        }

        public static void SetTicket(string accountID, bool persist)
        {
            FormsAuthentication.SetAuthCookie(accountID, persist);
        }
    }
}
    