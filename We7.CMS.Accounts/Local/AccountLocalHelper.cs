using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common.PF;
using Thinkment.Data;
using We7.Framework;

namespace We7.CMS.Accounts
{
    [Serializable]
    [Helper("We7.AccountHelper")]
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {
        public static readonly string AccountSessionKey = "We7.Session.Account.Key";

        public Account GetAccountByLoginName(string loginName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", loginName);
            if (Assistant.Count<Account>(c) > 0)
            {
                List<Account> accounts = Assistant.List<Account>(c, null);
            }
        }

        public string[] Login(string username, string password)
        {
            string[] results = { "false", ""};
            Account act = GetAccountByLoginName(username);
        }
    }

}
