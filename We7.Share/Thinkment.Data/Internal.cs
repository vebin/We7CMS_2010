using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Thinkment.Data
{
    public class ObjectManager
    {
        EntityObject curObject;
        public EntityObject CurObject
        {
            get { return curObject; }
            set { curObject = value; }
        }

        Type objType;
        public Type ObjType
        {
            get { return objType; }
            set { objType = value; }
        }

        Database curDatabase;
        public Database CurDatabase
        {
            get { return curDatabase; }
            set { curDatabase = value; }
        }

        public int MyCount(IConnection conn, Criteria condition)
        {
            CountHandle cs = new CountHandle();
            cs.Connect = conn;
            cs.ConditionCriteria = condition;
            cs.EntityObject = CurObject;
            cs.Execute();
            return cs.ReturnCode;
        }
    }

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
        public Dictionary<string, Property> PropertyDict
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

        bool isDataTable;
        public bool IsDataTable
        {
            get { return isDataTable; }
            set { isDataTable = value; }
        }

        Type typeForDT;
        public Type TypeForDT
        {
            get { return typeForDT; }
            set { typeForDT = value; }
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    class Dictionaries
    {
        Dictionary<string, IDatabase> databaseDict;
        Dictionary<IDatabase, IConnection> connectionDict;
        Dictionary<Type, ObjectManager> objectManagerDict;
        Dictionary<string, ObjectManager> objColumnDict;
        string GlobalDBString = string.Empty;
        string GlobalDBDriver = string.Empty;

        public Dictionaries()
        {
            databaseDict = new Dictionary<string, IDatabase>();
            connectionDict = new Dictionary<IDatabase, IConnection>();
            objectManagerDict = new Dictionary<Type, ObjectManager>();
            objColumnDict = new Dictionary<string, ObjectManager>();
        }

        public IConnection GetDBConnection(Type type)
        {
            return GetObjectManager(type).CurDatabase.CreateConnection();
        }

        public ObjectManager GetObjectManager(Type type)
        {
            if (!objectManagerDict.ContainsKey(type))
                throw new DataException(ErrorCodes.UnkownObject);
            return objectManagerDict[type];
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
                        ObjectManager om = new ObjectManager();
                        om.CurObject = _f4;
                        om.CurObject.IsDataTable = false;
                        om.CurObject.TypeForDT = null;
                        om.ObjType = _f4.ObjType;
                        om.CurDatabase = _f3;
                        objectManagerDict.Add(om.ObjType, om);
                    }
                    foreach (EntityObject _f4 in _f3.Objects.Values)
                    {
                        EntityObject _f5 = new EntityObject();
                        _f5 = _f4.Clone() as EntityObject;
                        ObjectManager om = new ObjectManager();
                        om.CurObject = _f5;
                        om.CurDatabase = _f3;
                        om.CurObject.IsDataTable = true;
                        om.CurObject.TypeForDT = typeof(TableInfo);
                        om.ObjType = typeof(TableInfo);
                        objColumnDict.Add(_f5.TableName, om);
                    }
                }
            }
        }

        private bool InArray(string[] src, string temp)
        {
            foreach (string ar in src)
            {
                if (String.Compare(ar, temp, true) == 0)
                    return true;
            }
            return false;
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

    abstract class BaseHandle
    {
        int parametersCount;

        string condition;
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        Criteria criteria;
        public Criteria ConditionCriteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        IConnection connect;
        public IConnection Connect
        {
            get { return connect; }
            set { connect = value; }
        }

        EntityObject entityObject;
        public EntityObject EntityObject
        {
            get { return entityObject; }
            set { entityObject = value; }
        }

        protected string Prefix
        {
            get { return connect.Driver.Prefix; }
        }

        SqlStatement sql;
        public SqlStatement SQL
        {
            get { return sql; }
        }

        int returnCode;
        public int ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }

        protected abstract void Build();

        public BaseHandle()
        {
            sql = new SqlStatement();
        }

        protected string AddParameter(Property p, object value)
        {
            string _f0 = string.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = p.Type;
            _f1.ParameterName = _f0;//？？？？
            _f1.Value = value;
            _f1.SourceColumn = p.Field;
            _f1.Size = p.Size;
            _f1.IsNullable = p.Nullable;
            SQL.Parameters.Add(_f1);
            return _f0;
        }

        protected void BuildCondition()
        {
            if (criteria != null)
            {
                condition = MakeCondition(criteria);
            }
        }

        string MakeCondition(Criteria c)
        {
            StringBuilder _f0 = new StringBuilder();
            if (c.Type != CriteriaType.None)
            {
                if (!EntityObject.PropertyDict.ContainsKey(c.Name))
                {
                    throw new DataException("No such field in object. " + c.Name);
                }
                if (c.Type == CriteriaType.IsNull)
                {
                    Property p = EntityObject.PropertyDict[c.Name];
                    _f0.Append(string.Format("{0} Is Null ", connect.Driver.FormatField(c.Adorn, p.Field, c.Start, c.Length)));
                }
                else if (c.Type == CriteriaType.IsNotNull)
                {
                    Property p = EntityObject.PropertyDict[c.Name];
                    _f0.Append(string.Format("{0} Is Not Null ", connect.Driver.FormatField(c.Adorn, p.Field, c.Start, c.Length)));
                }
                else
                {
                    string t = Connect.Driver.GetCriteria(c.Type);
                    Property p = EntityObject.PropertyDict[c.Name];
                    string pn = AddParameter(p, c.Value);
                    _f0.Append(string.Format(" {0} {1} {2} ", connect.Driver.FormatField(c.Adorn, p.Field, c.Start, c.Length), t, pn));
                }
            }
            if (c.Criterias.Count > 0)
            { 
                
            }
        }
    }

    class OperateHandle : BaseHandle
    {

        protected override void Build()
        {
            
        }
    }

    class CountHandle : OperateHandle
    {
        public void Execute()
        {
            Build();
        }

        protected override void Build()
        {
            SQL.SqlClause = string.Format("SELECT COUNT(*) FROM {0}", Connect.Driver.FormatTable(EntityObject.TableName));
            if (ConditionCriteria != null && !(ConditionCriteria.Type == CriteriaType.None && ConditionCriteria.Criterias.Count == 0))
            {
                BuildCondition();
                SQL.SqlClause += " WHERE " + Condition;
            }
        }
    }
}
