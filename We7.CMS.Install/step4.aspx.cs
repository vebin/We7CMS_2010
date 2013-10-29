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
            if (!IsPostBack)
            {
                DisableSubmitButton(this, this.ResetDBInfo);
            }
        }

        protected void PrevPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("step3.aspx", true);
        }

        protected void ResetDBInfo_Click(object sender, EventArgs e)
        { 
            
        }
    }
}
