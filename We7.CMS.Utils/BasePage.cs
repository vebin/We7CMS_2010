using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using We7.CMS.Common.Enum;
using We7.CMS.Utils.Helpers;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS
{
    public class BasePage : Page
    {
        public string AppPath
        {
            get 
            {
                if (MasterPageIs == MasterPageMode.User)
                    return "";
                else
                    return "/admin";
            }
        }

        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        protected virtual MasterPageMode MasterPageIs
        {
            get { return MasterPageMode.FullMenu; }
        }

        protected virtual bool NeedAnAccount
        {
            get { return true; }
        }

        protected virtual bool NeedAnPermission
        {
            get { return true; }
        }

        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Response.Expires = -1;
                if (!IsPostBack)
                {
                    Initialize();
                }
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BasePage), ex);
            }
        }

        protected virtual void Initialize()
        { 
        }
    }
}
