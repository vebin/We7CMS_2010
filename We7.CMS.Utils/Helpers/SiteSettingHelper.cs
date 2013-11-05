using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Utils.Helpers
{
    [Helper("We7.CMS.Helper")]
    public class SiteSettingHelper
    {
        public GeneralConfigInfo Config
        {
            get { return GeneralConfigs.GetConfig(); }
        }
    }
}
