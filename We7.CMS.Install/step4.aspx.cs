using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Config;
using System.Web;
using System.Web.UI.WebControls;

namespace We7.CMS.Install
{
    public class Step4 : SetupPage
    {
        protected Button ResetDBInfo;
        protected Button PrevPage;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (SetupPage.LockFileExist() && BaseConfigs.ConfigFileExist())     //这个判断条件怎么理解，与index.aspx.cs中为何不同？
                Init();
            else
                Response.Redirect("upgrade.aspx", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                if (context.Request["isforceload"] == "1")
                    ResetDBInfo.Enabled = false;
            }
            if (!Page.IsPostBack)
            {
                DisableSubmitButton(this, this.ResetDBInfo);    //对此函数的作用不理解，客户端js函数也找不到。
            }
        }

        protected void PrevPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("step3.aspx");
        }

        protected void ResetDBInfo_Click(object sender, EventArgs e)
        {
            BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
            if (bci.DBConnectionString != "" && bci.DBType != "")
            {
                Installer.ExecuteSQLGroup(bci);
                Response.Redirect("succeed.aspx?from=install");
            }
        }
    }
}
