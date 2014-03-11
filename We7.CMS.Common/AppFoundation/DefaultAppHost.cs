using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.AppFoundation
{
    public class DefaultAppHost : IAppHost
    {

        public string DirName
        {
            get { return "Apps"; }
        }
    }
}
