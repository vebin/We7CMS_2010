using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using System.Web;
using Thinkment.Data;

namespace We7.CMS.Accounts
{
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper 
    {
        public List<string> GetObjectsByPermission(string accountID, string permission)
        {
            List<string> channels = new List<string>();
            object tmpObj = HttpContext.Current.Session[accountID+"MyPermissionChannelList"+permission];
            if (tmpObj != null)
            {
                channels = (List<string>)tmpObj;
            }
            else
            {
                IAccountHelper ah = AccountFactory.CreateInstance();
                List<string> allowOwners = ah.GetRolesOfAccount(accountID);
                allowOwners.Add(accountID);
                channels = GetObjectID(allowOwners, permission);
                HttpContext.Current.Session[accountID + "MyPermissionChannelList" + permission] = channels;
            }
            return channels;
        }

        List<string> GetObjectID(List<string> ownerIDs, string level)
        { 
            List<string> levels = new List<string>();
            levels.Add(level);
            return GetObjectID(ownerIDs, levels);
        }

        List<string> GetObjectID(List<string> ownerIDs, List<string> levels)
        {
            List<string> list = new List<string>();
            List<Permission> permissions = GetPermissions(ownerIDs, levels);
            foreach (Permission p in permissions)
            {
                list.Add(p.ObjectID);
            }
            return list;
        }

        public List<Permission> GetPermissions(List<string> allowOwners, List<string> levels)
        {
            List<string> list = new List<string>();
            Criteria c = new Criteria(CriteriaType.None);
            if (allowOwners.Count != 0)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                foreach (string owner in allowOwners)
                {
                    subC.AddOr(CriteriaType.Equals, "OwnerID", owner);
                }
                c.Criterias.Add(subC);
            }
            if (levels != null)
            {
                Criteria subLevel = new Criteria(CriteriaType.None);
                foreach (string level in levels)
                {
                    subLevel.AddOr(CriteriaType.Equals, "Content", level);
                }
                c.Criterias.Add(subLevel);
            }
            List<Permission> ps = Assistant.List<Permission>(c, null);
            return ps;
        }
    }
}
