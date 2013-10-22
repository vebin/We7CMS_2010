using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS;

namespace We7.CMS.Web
{
    public partial class ShowSitePage : FrontBasePage
    {

        protected override string GoHandler
        {
            get
            {
                return "site";
            }
        }

        protected override string ColumnMode
        {
            get
            {
                if (Request["mode"] != null)
                    return Request["mode"].ToString();
                else
                    return "";
            }
        }

        protected override string TemplatePath
        {
            get;
            set;
        }

        protected override void Initialize()
        {
            base.Initialize();
            //string result = IsHtmlTemplate ? 
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}