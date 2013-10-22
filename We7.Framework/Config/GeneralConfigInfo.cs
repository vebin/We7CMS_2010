using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Config
{
    [Serializable]
    public class GeneralConfigInfo : IConfigInfo
    {
        bool onlyLoginUserCanVisit = false;
        public bool OnlyLoginUserCanVisit
        {
            get { return onlyLoginUserCanVisit; }
            set { onlyLoginUserCanVisit = value; }
        }

        bool enableHtmlTemplate = true;
        public bool EnableHtmlTemplate
        {
            get { return enableHtmlTemplate; }
            set { enableHtmlTemplate = value; }
        }
    }
}
