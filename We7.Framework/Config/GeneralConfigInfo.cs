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
        string defaultHomePageTitle = "首页 - We7站点";
        
        string defaultTemplateGroup, mobileTemplateGroupFileName, tabletTemplateGroupFileName;
        string defaultTemplateGroupFileName;
        bool enableHtmlTemplate = true;
        string enableLoginAuhenCode;
        bool enableCookieAuthentication = true;
        bool isOEM = false;
        string icpInfo = "";
        string iPDBConnection = "New=False;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={$Current}\\IP.db";

        bool isAddLog;
        
        
        bool onlyLoginUserCanVisit = false;
        string productName = "We7";
        static string productVersion = "V" + AssemblyFileVersion.ProductVersion + "正式版";

        string siteTitle = "We7";
        string siteFullName = "西部动力（北京）科技有限公司";
        string _siteLogo = "";
        string siteSkinsBasePath = "_skins";
        
        
        string _SSOSiteUrl = string.Empty;
        string userRegisterMode = "none";

        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }
        public string DefaultHomePageTitle
        {
            get { return defaultHomePageTitle; }
            set { defaultHomePageTitle = value; }
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
        public string IPDBConnection
        {
            get { return iPDBConnection; }
            set { iPDBConnection = value; }
        }
        public bool IsAddLog
        {
            get { return isAddLog; }
            set { isAddLog = value; }
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
        public string DefaultTemplateGroup
        {
            get { return defaultTemplateGroup; }
            set { defaultTemplateGroup = value; }
        }
        public string DefaultTemplateGroupFileName
        {
            get { return defaultTemplateGroupFileName; }
            set { defaultTemplateGroupFileName = value; }
        }
        public bool IsDemoSite
        {
            get;
            set;
        }
        public string MobileTemplateGroupFileName
        {
            get { return mobileTemplateGroupFileName; }
            set { mobileTemplateGroupFileName = value; }
        }
        public string TabletTemplateGroupFileName
        {
            get { return tabletTemplateGroupFileName; }
            set { tabletTemplateGroupFileName = value; }
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
        public string ProductVersion
        {
            get { return productVersion; }
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
        public string SiteSkinsBasePath
        {
            get { return siteSkinsBasePath; }
            set { siteSkinsBasePath = value; }
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
