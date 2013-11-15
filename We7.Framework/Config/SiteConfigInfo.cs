using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Config
{
    [Serializable]
    public class SiteConfigInfo : IConfigInfo
    {
        private string administratorName = "Administrator";
        private string administratorKey = "1";
        private string companyName = "您的站点";
        private bool isPasswordHashed = false;
        private bool siteGroupEnabled = false;
        private string siteid;


        public string SiteName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        public string AdministratorName
        {
            get { return administratorName; }
            set { administratorName = value; }
        }
        public string AdministratorKey
        {
            get { return administratorKey; }
            set { administratorKey = value; }
        }
        public bool IsPasswordHashed
        {
            get { return isPasswordHashed; }
            set { isPasswordHashed = value; }
        }
        public string PassportAuthPage 
        {
            get
            {
                if (!string.IsNullOrEmpty(PassportServiceUrl))
                {
                    string url = PassportServiceUrl.Remove(PassportServiceUrl.LastIndexOf("/"));
                    url += "/Authentication.aspx";
                    return url;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string PassportServiceUrl { get; set; }
        public bool SiteGroupEnabled
        {
            get { return siteGroupEnabled; }
            set { siteGroupEnabled = value; }
        }
        public string SiteID
        {
            get { return siteid; }
            set { siteid = value; }
        }
        
    }
}
