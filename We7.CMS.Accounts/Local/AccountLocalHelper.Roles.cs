using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Accounts.Common.PF;
using Thinkment.Data;

namespace We7.CMS.Accounts
{
    public class AccountLocalHelper : BaseHelper, IAccountHelper
    {
        public List<AccountRole> GetAccountRoles(string accountID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            return Assistant.List<AccountRole>(c, null);
        }

        public List<string> GetRolesOfAccount(string accountID)
        {
            List<string> allOwners = new List<string>();
            List<AccountRole> accountRoles = GetAccountRoles(accountID);
            foreach (AccountRole ar in accountRoles)
            {
                allOwners.Add(ar.RoleID);
            }
            return allOwners;
        }
    }
}
