using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;
using We7.Framework;

namespace We7.CMS.Accounts
{
    public class AccountFactory
    {
        public static IAccountHelper CreateInstance()
        {
            if (SiteConfigs.GetConfig().SiteGroupEnabled)
            {
                if (string.IsNullOrEmpty(SiteConfigs.GetConfig().PassportServiceUrl))
                    throw new InvalidOperationException("您还没有在site.config中配置身份认证服务地址（PassportServiceUrl）的值！");
                AccountRemoteHelper ar = new AccountRemoteHelper();
                return ar;
            }
            else
            {
                AccountLocalHelper al = HelperFactory.Instance.GetHelper<AccountLocalHelper>();
                return al;
            }
        }

    }
}
