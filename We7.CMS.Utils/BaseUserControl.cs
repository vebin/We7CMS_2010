using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;

namespace We7.CMS
{
    [Serializable]
    public class BaseUserControl : System.Web.UI.UserControl
    {
        protected virtual string AccountID
        {
            get { return Security.CurrentAccountID; }
        }

        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected HelperFactory HelperFactory
        {
            get{ return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        protected Utils.SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<Utils.SiteSettingHelper>(); }
        }

        protected LogHelper LogHelper
        {
            get { return HelperFactory.GetHelper<LogHelper>(); }
        }

        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
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
