using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Install
{
    public class DatabaseInfo
    {
        private string initialCatalog;
        public string Server
        {
            get { return initialCatalog; }
            set { initialCatalog = value; }
        }

        private string datasource;
        public string Database
        { 
            get
            {
                if (datasource != null && datasource != "")
                {
                    return datasource;
                }
                else if (dataFile != null && dataFile != "")
                {
                    if (dataFile.IndexOf("\\") > -1)
                    {
                        return dataFile.Substring(dataFile.LastIndexOf("\\")+1);
                    }
                }
                return datasource;
            }
            set{datasource = value;}
        }

        string dataFile;
        public string DBFile
        {
            get { return dataFile; }
            set { dataFile = value; }
        }

        string userID;
        public string User
        {
            get { return userID; }
            set { userID = value; }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
