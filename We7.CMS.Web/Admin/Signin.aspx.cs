using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class Signin : BasePage
    {
        public string ProductBrand
        {
            get 
            {
                GeneralConfigInfo gi = GeneralConfigs.GetConfig();
                if (gi != null)
                {
                    return gi.ProductName;
                }
                else
                    return "We7";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}