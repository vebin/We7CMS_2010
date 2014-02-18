using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Accounts.Common.PF
{
    [Serializable]
    public class AccountRole
    {
        string roleID;

        public string RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

    }
}
