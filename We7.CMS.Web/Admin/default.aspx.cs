using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace We7.CMS.Web.Admin
{
    public partial class Administration : BasePage
    {
        protected override bool NeedAnAccount
        {
            get
            {
                return true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "/admin/"+Constants.ThemePath+"/main.aspx";
            Response.Redirect(url, false);
        }
    }
}