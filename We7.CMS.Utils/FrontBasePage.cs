using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.CMS.Accounts;
using System.Web;

namespace We7.CMS
{
    public class FrontBasePage : Page
    {

        protected virtual string GoHandler { get { return ""; } }

        protected virtual string ColumnMode { get { return ""; } }

        protected virtual string TemplatePath { get; set; }

        protected bool IsHtmlTemplate
        {
            get
            {
                return GeneralConfigs.GetConfig().EnableHtmlTemplate && String.IsNullOrEmpty(Request["CreateHtml"]);
            }
        }

        protected virtual void Initialize()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            Response.Expires = -1;  //设置浏览器缓存页面过期时间的优势，为什么要设置？（参考二十八条改善 ASP 性能和外观的技巧！）
            base.OnLoad(e);

            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("您的数据库配置文件尚未生成，看起来数据库尚未建立，您需要建立数据库配置文件或生成数据库。现在开始吗？<a href='/install/index.aspx'><u>现在配置数据库</u></a>");
                Response.End();
            }
            if (UnLoginCheck() && Request.RawUrl.ToLower().Contains("login.aspx"))
            {
                We7.Framework.Util.Utils.Redirect("/login.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
            }

            Initialize();
        }

        protected bool UnLoginCheck()
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            return gi.OnlyLoginUserCanVisit && !Security.IsAuthenticated();
        }
    }
}
