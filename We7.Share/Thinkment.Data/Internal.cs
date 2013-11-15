using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Data;

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

        public List<T> MyList<T>(IConnection conn, Criteria condition, Order[] orders, int from, int count, string[] fields)
        {
            ListField[] fs = null;
            if (fields != null && fields.Length > 0)
            {
                fs = new ListField[fields.Length];
                for (int i=0; i<fields.Length; i++)
                {
                    fs[i] = new ListField(fields[i]);
                }
            }
            return MyList<T>(conn, condition, orders, from, count, fs);
        }

        public List<T> MyList<T>(IConnection conn, Criteria condition, Order[] orders, int from, int count, ListField[] fields)
        {
            if (typeof(T) != ObjType)
                throw new DataException(ErrorCodes.UnmatchType);
            ListSelectHandle<T> ls = new ListSelectHandle<T>();
            ls.Connect = conn;
            ls.ConditionCriteria = condition;
            ls.EntityObject = curObject;
            ls.From = from;
            ls.Count = count;

            if (orders != null)
            {
                foreach (Order order in orders)
                {
                    ls.OrderList.Add(order);
                }
            }
            if (fields != null)
            {
                foreach (ListField field in fields)
                {
                    if (!ls.ListFieldDict.ContainsKey(field.FieldName))
                    {
                        ls.ListFieldDict.Add(field.FieldName, field);
                    }
                }
            }
            ls.Execute();

            return ls.Data;
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

        public List<Order> BuildOrderList()
        {
            string[] _f0 = primaryKeyName.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            if (_f0.Length == 0)
                throw new DataException(ErrorCodes.NoPrimaryKey);
            List<Order> _f1 = new List<Order>();
            foreach (string _f2 in _f0)
            {
                if (!propertyDict.ContainsKey(_f2))
                    throw new DataException(ErrorCodes.UnkownProperty);
                _f1.Add(new Order(propertyDict[_f2].Field));
            }
            return _f1;
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

        public void SetValue(object obj, string name, object value)
        {
            if (!propertyDict.ContainsKey(name))
                throw new DataException(name, ErrorCodes.UnkownProperty);
            Property p = propertyDict[name];
            if (p != null && value != null)
            {
                if (p.Info.PropertyType.Name.ToLower() == "int32" &&
                    value.GetType().Name.ToLower() == "decimal")
                    value = Convert.ToInt32(value);
                else if (p.Info.PropertyType.Name.ToLower() == "int64" &&
                    value.GetType().Name.ToLower() == "decimal")
                    value = Convert.ToInt64(value);
            }
            if (value == null && p != null)
            {
                if (p.Info.PropertyType.Name.ToLower() == "string")
                    value = string.Empty;
                else if (p.Info.PropertyType.Name.ToLower() == "int32" ||
                    p.Info.PropertyType.Name.ToLower() == "int64")
                    value = -1;
            }
            else
            { 
                
            }

            p.Info.SetValue(obj, value, null);
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

        public ObjectManager GetObjectManager(string tablename)
        {
            if (!objColumnDict.ContainsKey(tablename))
                throw new DataException(ErrorCodes.UnkownObject);
            return objColumnDict[tablename];
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

        public void AddSplitString(StringBuilder sb, string s)
        {
            if (sb.Length > 0)
            {
                sb.Append(",");
            }
            sb.Append(s);
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
                string _f1 = c.Mode == CriteriaMode.And ? "AND" : "OR";
                if (c.Type != CriteriaType.None)
                    _f0.Append(_f1);
                if (c.Criterias.Count > 1)
                    _f0.Append("(");
                _f0.Append(MakeCondition(c.Criterias[0]));
                for (int i = 1; i < c.Criterias.Count; i++)
                {
                    _f0.Append(_f1);
                    _f0.Append(MakeCondition(c.Criterias[i]));
                }
                if (c.Criterias.Count > 1)
                    _f0.Append(")");
            }

            return _f0.ToString();
        }
    }

    class OperateHandle : BaseHandle
    {
        public OperateHandle()
        {
            orderlist = new List<Order>();
            listFieldDict = new Dictionary<string, ListField>(StringComparer.OrdinalIgnoreCase);
        }

        List<Order> orderlist;

        public List<Order> OrderList
        {
            get { return orderlist; }
        }

        Dictionary<string, ListField> listFieldDict;
        public Dictionary<string, ListField> ListFieldDict
        {
            get { return listFieldDict; }
        }

        private string fields;
        public string Fields
        {
            get { return fields; }
            set { fields = value; }
        }


        protected override void Build()
        {
            
        }

        protected void BuildField(bool allowReadonly)
        {
            StringBuilder _f0 = new StringBuilder();

            foreach (Property p in EntityObject.PropertyDict.Values)
            {
                Adorns a = Adorns.None;
                if (!allowReadonly && p.Readonly)
                    continue;
                if (listFieldDict.Count > 0)
                {
                    if (ListFieldDict.ContainsKey(p.Name))
                    {
                        a = ListFieldDict[p.Name].Adorn;
                    }
                    else if (listFieldDict.ContainsKey(p.Field.ToUpper()))
                    {
                        a = ListFieldDict[p.Field.ToUpper()].Adorn;
                    }
                    else if (listFieldDict.ContainsKey(p.Field.ToLower()))
                    {
                        a = listFieldDict[p.Field.ToLower()].Adorn;
                    }
                    else
                    {
                        continue;
                    }
                }
                AddSplitString(_f0, Connect.Driver.FormatField(a, p.Field));
            }

            fields = _f0.ToString();
        }

        protected void BindObject(object o, DataRow dr)
        {
            foreach (Property p in EntityObject.PropertyDict.Values)
            {
                if (listFieldDict.Count > 0 && !listFieldDict.ContainsKey(p.Field))
                    continue;
                object v = dr[p.Field];
                if (v == DBNull.Value)
                    v = null;
                EntityObject.SetValue(o, p.Name, v);
            }
        }
    }

    class CountHandle : OperateHandle
    {
        public void Execute()
        {
            Build();
            ReturnCode = Convert.ToInt32(Connect.QueryScalar(SQL));
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

    class ListSelectHandle<T> : OperateHandle
    {
        public ListSelectHandle()
        {
            _data = new List<T>();

        }        

        private int _from;
        public int From
        {
            get { return _from; }
            set { _from = value; }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        private List<T> _data;
        public List<T> Data
        {
            get { return _data; }
        }

        private DataTable table;
        public DataTable Table
        {
            get { return table; }
            set { table = value; }
        }


        protected override void Build()
        {
            BuildField(true);
            if (ConditionCriteria != null && !(ConditionCriteria.Type == CriteriaType.None && ConditionCriteria.Criterias.Count == 0))
                BuildCondition();
            else
                Condition = string.Empty;
            List<Order> os = new List<Order>();
            foreach (Order order in OrderList)
            {
                if (!EntityObject.PropertyDict.ContainsKey(order.Name))
                {
                    string msg = string.Format("Property '{0}' doesn't belongs to '{1}'", order.Name, EntityObject.TypeName);
                    throw new DataException(msg, ErrorCodes.UnkownProperty);
                }
                Property p = EntityObject.PropertyDict[order.Name];
                os.Add(new Order(p.Field, order.Mode));
            }
            if (os.Count == 0 || From <= 0)
                os = EntityObject.BuildOrderList();
            SQL.SqlClause = Connect.Driver.BuildPaging(Connect.Driver.FormatTable(EntityObject.TableName), Fields, Condition, os, From, Count);
        }

        public void Execute()
        {
            _data.Clear();
            Build();
            DataTable dt = Connect.Query(SQL);
            if (EntityObject.IsDataTable)
            {
                dt.TableName = EntityObject.TableName;
                table = dt;
                object o = new TableInfo(table, EntityObject.PropertyDict);
                _data.Add((T)o);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    object o = Activator.CreateInstance(EntityObject.ObjType);
                    BindObject(o, dr);
                    _data.Add((T)o);
                }
            }
        }
    }
}
