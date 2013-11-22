﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;

namespace We7.CMS.Utils
{
    [Serializable]
    public class BaseUserControl : System.Web.UI.UserControl
    {
        protected virtual string AccountID
        {
            get { return Security.CurrentAccountID; }
        }

        protected HelperFactory HelperFactory
        {
            get{ return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        protected LogHelper LogHelper
        {
            get { return HelperFactory.GetHelper<LogHelper>(); }
        }

        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        protected bool DemoSiteMessage
        {
            get 
            {
                if (GeneralConfigs.GetConfig().IsDemoSite)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "<script>alert('对不起，演示站点禁止保存！')</script>");
                    return true;
                }
                return false;
            }
        }

        protected void AddLog(string pages, string content)
        {
            if (CDHelper.Config.IsAddLog)
                LogHelper.WriteLog(AccountID, pages, content, CDHelper.Config.DefaultHomePageTitle);
        }

    }
}
