using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace We7.CMS.Accounts
{
    public class Security
    {
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
    }
}
    