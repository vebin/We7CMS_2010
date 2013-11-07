using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Thinkment.Data
{
    class Dictionaries
    {
        string GlobalDBString = string.Empty;
        string GlobalDBDriver = string.Empty;

        public void SetGlobalDBString(string dbString, string dbDriver)
        {
            GlobalDBString = dbString;
            GlobalDBDriver = dbDriver;
        }

        public void LoadDataSource(string root, string[] dbs)
        {
            if (Directory.Exists(root))
            {
                DirectoryInfo dir = new DirectoryInfo(root);
                FileInfo[] files = dir.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
                    LoadDatabases(file.FullName, dbs);
                }
            }
        }

        public void LoadDatabases(string fullname, string[] dbs)
        {
            XmlDocument _f0 = new XmlDocument();
            _f0.Load(fullname);
            if (_f0.SelectNodes("//DbConnectionString").Count > 0)
            {
                foreach (XmlNode node in _f0.SelectNodes("//DbConnectionString"))
                {
                    Database _f3 = new Database();
                }
            }
        }
    }

    public class Database : IDatabase
    { 
        
    }
}
