using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;

namespace We7.CMS.Utils
{
    [Serializable]
    public class BaseUserControl : System.Web.UI.UserControl
    {
        protected HelperFactory HelperFactory
        {
            get{ return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }


    }
}
