using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    public class AccountRemoteHelper : IAccountHelper
    {

        public string[] Login(string username, string password)
        {
            string[] results = { "", ""};
            if (HttpContext.Current.Request["Authenticator"] != null)
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.Action = "signin";
                ssoRequest.UserName = username;
                ssoRequest.Password = password;
                ssoRequest.SiteID = SiteConfigs.GetConfig().SiteID;
                Authentication.CreateAppToken(ssoRequest);
                Authentication.Post(ssoRequest, SiteConfigs.GetConfig().PassportAuthPage);
            }

            return results;
        }


        public List<string> GetObjectsByPermission(string accountID, string permission)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRolesOfAccount(string accountID)
        {
            throw new NotImplementedException();
        }
    }
}
