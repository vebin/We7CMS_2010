using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Thinkment.Data
{
    static class UpdateXmlElement
    {
        public static string GetXEAttribute(XmlElement element, string name, string value)
        {
            if (element.HasAttribute(name))
                return element.GetAttribute(name);
            return value;
        }

        public static int GetXEAttribute(XmlElement element, string name, int value)
        {
            if (element.HasAttribute(name))
                return Convert.ToInt32(element.GetAttribute(name));
            return value;
        }

        public static bool GetXEAttribute(XmlElement element, string name, bool value)
        {
            if (element.HasAttribute(name))
                return Convert.ToBoolean(element.GetAttribute(name));
            return value;
        }
    }

    public class EntityObject : ICloneable
    {
        public EntityObject()
        {
            propertyDict = new Dictionary<string, Property>(StringComparer.OrdinalIgnoreCase);
        }
        string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        string tableName;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        string primaryKeyName;
        public string PrimaryKeyName
        {
            get { return primaryKeyName; }
            set { primaryKeyName = value; }
        }

        string identityName;
        public string IdentityName
        {
            get { return identityName; }
            set { identityName = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        Dictionary<string, Property> propertyDict;
        private Dictionary<string, Property> PropertyDict
        {
            get { return propertyDict; }
            set { propertyDict = value; }
        }

        Type objType;
        public Type ObjType
        {
            get { return objType; }
            set { objType = value; }
        }

        public void Build()
        {
            objType = Type.GetType(typeName);
            if (objType == null)
                throw new DataException(ErrorCodes.UnkownObject);
            foreach (PropertyInfo pi in objType.GetProperties())
            {
                if (propertyDict.ContainsKey(pi.Name))
                    propertyDict[pi.Name].Info = pi;
            }
            foreach (Property p in propertyDict.Values)
            {
                if (p.Info == null)
                {
                    string s = string.Format("{0}.{1}", ObjType.ToString(), p.Name);
                    throw new DataException(s, ErrorCodes.UnkownProperty);
                }
                    
            }
        }

        public EntityObject FromXml(XmlElement element)
        {
            propertyDict.Clear();
            typeName = UpdateXmlElement.GetXEAttribute(element, "type", "");    //此方法用于此处根本就是庸人自扰
            tableName = UpdateXmlElement.GetXEAttribute(element, "table", "");
            primaryKeyName = UpdateXmlElement.GetXEAttribute(element, "primaryKey", "");
            description = UpdateXmlElement.GetXEAttribute(element, "description", "");
            foreach (XmlElement xe in element.SelectNodes("Property"))
            {
                Property p = new Property().FromXml(xe);
                propertyDict.Add(p.Name, p);
            }

            return this;
        }
    }

    class Dictionaries
    {
        Dictionary<string, IDatabase> databaseDict;
        Dictionary<IDatabase, IConnection> connectionDict;
        string GlobalDBString = string.Empty;
        string GlobalDBDriver = string.Empty;

        public Dictionaries()
        {
            databaseDict = new Dictionary<string, IDatabase>();
            connectionDict = new Dictionary<IDatabase, IConnection>();
        }

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
        /// <summary>
        /// 建立字典集合类，包含数据库集合、数据表集合、表字段集合、数据库连接集合；
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="dbs"></param>
        public void LoadDatabases(string fullname, string[] dbs)
        {
            XmlDocument _f0 = new XmlDocument();
            _f0.Load(fullname);
            if (_f0.SelectNodes("//DbConnectionString").Count > 0)
            {
                foreach (XmlNode node in _f0.SelectNodes("//DbConnectionString"))
                {
                    Database _f3 = new Database();
                    _f3.ConnectionString = node.Attributes["value"].Value;
                    _f3.Driver = node.Attributes["driver"].Value;
                    databaseDict.Add(node.Attributes["key"].Value, _f3);
                }
            }
            else
            {
                foreach (XmlElement node in _f0.SelectNodes("Objects/Database"))
                {
                    FileInfo _f2 = new FileInfo(fullname);
                    Database _f3 = new Database();
                    _f3.FromXml(node);

                    if (dbs != null && !InArray(dbs, _f3.Name))
                        continue;
                    if (GlobalDBString != string.Empty)
                    {
                        _f3.ConnectionString = GlobalDBString;
                        _f3.Driver = GlobalDBDriver;
                    }

                    _f3.ConnectionString = _f3.ConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                    _f3.ConnectionString = _f3.ConnectionString.Replace("{$Current}", Path.GetDirectoryName(_f2.FullName));
                    databaseDict.Add(_f3.Name, _f3);
                    connectionDict.Add(_f3, _f3.CreateConnection());

                    foreach (EntityObject _f4 in _f3.Objects.Values)
                    {
                        _f4.Build();
                    }
                }
            }
        }
    }

    [Serializable]
    public class Database : IDatabase
    {
        string connectStr;
        public string ConnectionString
        {
            get { return connectStr; }
            set { connectStr = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string driver;
        public string Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        Dictionary<string, EntityObject> objects;
        public Dictionary<string, EntityObject> Objects
        {
            get { return objects; }
        }

        IDbDriver dbDriver;
        public IDbDriver DbDriver
        {
            get { return dbDriver; }
            set { dbDriver = value; }
        }


        public Database()
        {
            objects = new Dictionary<string, EntityObject>();
        }

        public IConnection CreateConnection()
        {
            return DbDriver.CreateConnection(ConnectionString);
        }

        public Database FromXml(XmlElement element)
        {
            Objects.Clear();
            name = element.GetAttribute("name");
            driver = element.GetAttribute("driver");
            connectStr = element.GetAttribute("connectionString");
            foreach (XmlElement xe in element.SelectNodes("Object"))
            {
                EntityObject item = new EntityObject().FromXml(xe);
                Objects.Add(item.TypeName, item);
            }

            return this;
        }
    }
}
