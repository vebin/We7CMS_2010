using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Config
{
    [Serializable]
    public class SiteConfigInfo : IConfigInfo
    {
        private string companyName = "您的站点";
        public string SiteName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string administratorName = "Administrator";
        public string AdministratorName
        {
            get { return administratorName; }
            set { administratorName = value; }
        }

        private string administratorKey = "1";
        public string AdministratorKey
        {
            get { return administratorKey; }
            set { administratorKey = value; }
        }

        private bool isPasswordHashed = false;
        public bool IsPasswordHashed
        {
            get { return isPasswordHashed; }
            set { isPasswordHashed = value; }
        }
    }
}
