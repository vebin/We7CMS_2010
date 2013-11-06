using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace We7.Framework.Config
{
    [Serializable]
    public class GeneralConfigInfo : IConfigInfo
    {
        static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        string copyright = "Powered by <a href=\"http://we7.cn/\" target=\"_blank\">We7</a> " + productVersion + " ©2011 <a href=\"http://www.westengine.com/\" target=\"_blank\">WestEngine Inc.</a>";
        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }

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

        bool isOEM = false;
        public bool IsOEM
        {
            get { return isOEM; }
            set { isOEM = value; }
        }

        string productName = "We7";
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string CopyrightOfWe7
        {
            get
            {
                return "Powered by <a href=\"http://we7.cn/\" target=\"_blank\">We7</a> " + productVersion + " ©2011 <a href=\"http://www.westengine.com/\" target=\"_blank\">WestEngine Inc.</a>";
            }
        }

        bool enableCookieAuthentication = true;
        public bool EnableCookieAuthentication
        {
            get { return enableCookieAuthentication; }
            set { enableCookieAuthentication = value; }
        }

        string enableLoginAuhenCode;
        public string EnableLoginAuhenCode
        {
            get { return enableLoginAuhenCode; }
            set { enableLoginAuhenCode = value; }
        }

        static string productVersion = "V"+AssemblyFileVersion.ProductVersion+"正式版";

        string _SSOSiteUrl = string.Empty;
        public string SSOSiteUrl
        {
            get { return _SSOSiteUrl; }
            set { _SSOSiteUrl = value; }
        }

    }
}
