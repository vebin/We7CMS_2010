using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Install
{
    public class succeed : SetupPage
    {
        protected System.Web.UI.WebControls.Literal SummaryLiteral;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Init();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string summary = @"恭喜！您已经成功安装{0}<br/><br/>
                                        请牢记以下您的个人信息<br/><br/>
                                      用户名：{1}<br/>
                                      密码：{2}<br/>";
                if (Request["from"] != null && Request["from"] == "install")
                {
                    SummaryLiteral.Text = string.Format(summary, productName, Session["SystemAdminName"], Session["SystemAdminPwd"]);
                }
                else
                {
                    SummaryLiteral.Text = "";
                }
                DeleteLockFile();
            }
        }
    }
}
