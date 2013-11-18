using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class NewSiteWizard : BasePage
    {
        protected override MasterPageMode MasterPageIs        {
            get             {
                if (Request["nomenu"] != null)
                    return MasterPageMode.NoMenu;
                else
                    return MasterPageMode.FullMenu;            }        }

        protected override void Initialize()
        {
            if (Request["nomenu"] != null)
            {

            }
            else
            { 
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string CheckLength(string content, int len)
        {
            if (content.Length > len)
                return string.Format("{0}...", content.Substring(0, len));
            else
                return content;
        }
    }
}