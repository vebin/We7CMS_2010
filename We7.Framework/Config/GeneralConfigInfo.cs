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
        bool enableHtmlTemplate = true;
        string enableLoginAuhenCode;
        bool enableCookieAuthentication = true;
        bool isOEM = false;
        string icpInfo = "";
        
        bool onlyLoginUserCanVisit = false;
        string productName = "We7";
        static string productVersion = "V" + AssemblyFileVersion.ProductVersion + "正式版";

        string siteTitle = "We7";
        string siteFullName = "西部动力（北京）科技有限公司";
        string _siteLogo = "";
        
        string _SSOSiteUrl = string.Empty;
        string userRegisterMode = "none";



        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }
        public bool OnlyLoginUserCanVisit
        {
            get { return onlyLoginUserCanVisit; }
            set { onlyLoginUserCanVisit = value; }
        }
        public bool EnableHtmlTemplate
        {
            get { return enableHtmlTemplate; }
            set { enableHtmlTemplate = value; }
        }
        public bool IsOEM
        {
            get { return isOEM; }
            set { isOEM = value; }
        }
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
        public bool EnableCookieAuthentication
        {
            get { return enableCookieAuthentication; }
            set { enableCookieAuthentication = value; }
        }
        public string EnableLoginAuhenCode
        {
            get { return enableLoginAuhenCode; }
            set { enableLoginAuhenCode = value; }
        }
        public string IcpInfo
        {
            get { return icpInfo; }
            set { icpInfo = value; }
        }
        public string SiteFullName
        {
            get { return siteFullName; }
            set { siteFullName = value; }
        }
        public string SiteLogo
        {
            get { return _siteLogo; }
            set { _siteLogo = value; }
        }
        public string SiteTitle
        {
            get { return siteTitle; }
            set { siteTitle = value; }
        }
        public string SSOSiteUrl
        {
            get { return _SSOSiteUrl; }
            set { _SSOSiteUrl = value; }
        }
        public string UserRegisterMode
        {
            get { return userRegisterMode; }
            set { userRegisterMode = value; }
        }
    }
}
