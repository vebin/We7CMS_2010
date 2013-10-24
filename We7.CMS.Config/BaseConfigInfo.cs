using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS.Config
{
    [Serializable]
    public class BaseConfigInfo : IConfigInfo
    {
        private string dbConnectionString = "New=false;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={$Current}\\CD.DB3";
        public string DBConnectionString
        {
            get{ return dbConnectionString; }
            set { dbConnectionString = value; }
        }

        private string dbType = "SQLite";
        public string DBType
        {
            get { return dbType; }
            set { dbType = value; }
        }

        private string dbDriver = "Thinkment.Data.SQLiteDriver";       //命名规则：当首字母小写，后面属于同一单词的大写字母也小写，如DBDriver和dbDriver中的DB、db；
        public string DBDriver
        {
            get { return dbDriver; }
            set { dbDriver = value; }
        }
    }
}
