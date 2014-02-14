using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public class ObjectAssistant
    {
        Dictionaries _d1;

        public ObjectAssistant()
        {
            _d1 = new Dictionaries();
        }

        public void DeleteList<T>(Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                DeleteList<T>(conn, condition);
            }
        }

        public void DeleteList<T>(IConnection conn, Criteria condition)
        {
            int rowsAffected = 0;
            bool stoped = false;
            AssistantEntity typedEntity = new AssistantEntity(typeof(T));
            typedEntity.Criteria = condition;
            if (AssertContinueDeleting(typedEntity, out stoped))
            {
                ObjectManager oa = _d1.GetObjectManager(typeof(T));
                rowsAffected = oa.MyDeleteList(conn, condition);
            }
        }
   
        /// <summary>
        /// 按条件取得数据库记录列表（其中部分）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count)
        {
            return List<T>(condition, orders, from, count, (string[])null);   
        }

        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count, string[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return List<T>(conn, condition, orders, from, count, fields);
            }
        }

        public List<T> List<T>(IConnection conn, Criteria condition, Order[] orders, int offset, int count, string[] fields)
        { 
            return SelectList<T>(conn, null, condition, orders, offset, count, fields);
        }

        public void LoadDBConnectionString(string connectStr, string dbDriver)
        {
            _d1.SetGlobalDBString(connectStr, dbDriver);
        }

        public void LoadDataSource(string dir)
        {
            _d1.LoadDataSource(dir, null);
        }

        public int Count<T>(Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return Count<T>(conn, condition);
            }
        }

        public int Count<T>(IConnection conn, Criteria condition)
        {
            ObjectManager om = _d1.GetObjectManager(typeof(T));
            return om.MyCount(conn, condition);
        }

        public object Insert(object obj)
        {
            return Insert(obj, null);
        }

        public object Insert(object obj, string[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                return Insert(conn, obj, fields);
            }
        }

        public object Insert(IConnection conn, object obj, string[] fields)
        {
            int rowsAffected = 0;
            object identity = null;
            bool stoped = false;

            AssistantEntity typedEntity = new AssistantEntity(obj);
            if (AssertContinueInserting(typedEntity, fields, out stoped))
            {
                ObjectManager oa = _d1.GetObjectManager(obj.GetType());
                rowsAffected = oa.MyInsert(conn, obj, fields, out identity);
            }
            if (!stoped)
            {
 
            }

            return rowsAffected;
        }

        public List<T> List<T>(Criteria condition, Order[] orders)
        {
            return List<T>(condition, orders, 0, 0, (ListField[])null);
        }

        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count, ListField[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return List<T>(conn, condition, orders, from, count, fields);
            }
        }

        public List<T> List<T>(IConnection conn, Criteria condition, Order[] orders, int offset, int count, ListField[] fields)
        {
            return SelectList<T>(conn, null, condition, orders, offset, count, fields);
        }

        public int Update(object obj, string[] fields)
        {
            return Update(obj, fields, null);
        }

        public int Update(object obj, string[] fields, Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                return Update(conn, obj, fields, condition);
            }
        }

        public int Update(IConnection conn, object obj, string[] fields, Criteria condition)
        {
            bool stoped = false;
            int rowsAffected = 0;
            AssistantEntity typedEntity = new AssistantEntity(obj);
            typedEntity.Criteria = condition;

            if (AssertContinueUpdating(typedEntity, fields, out stoped))
            {
                ObjectManager oa = _d1.GetObjectManager(obj.GetType());
                rowsAffected = oa.MyUpdate(conn, obj, fields, condition);
            }

            if (!stoped)
            {
                //NotifyUpdated(typedEntity, fields, rowsAffected);
            }
            return rowsAffected;
        }

        [field: NonSerializedAttribute]
        public event EventHandler<AssistantPrepareEventArgs> PreDeleteItem;

        [field: NonSerializedAttribute]
        public event EventHandler<AssistantPrepareEventArgs> PreSelectItem;

        [field: NonSerializedAttribute]
        public event EventHandler<AssistantPrepareEventArgs> PreInsertItem;

        [field: NonSerializedAttribute]
        public event EventHandler<AssistantPrepareEventArgs> PreUpdateItem;


        private bool AssertContinueDeleting(AssistantEntity entity, out bool stoped)
        {
            stoped = false;
            if (null != PreDeleteItem)
            {
                AssistantPrepareEventArgs args = new AssistantPrepareEventArgs();
                args.Entity = entity;
                PreDeleteItem(this, args);

                stoped = args.Stoped;
                return !args.Canceled;
            }
            return true;
        }

        private bool AssertContinueInserting(AssistantEntity entity, string[] fields, out bool stoped)
        {
            stoped = false;
            if (null != PreInsertItem)
            {
                AssistantPrepareEventArgs args = new AssistantPrepareEventArgs();
                args.Entity = entity;
                args.Fields = fields;
                PreInsertItem(this, args);
                stoped = args.Stoped;
                return !args.Canceled;
            }
            return true;
        }

        private bool AssertContinueSelecting(AssistantEntity entity, AssistantQueryParameter parameter, string[] fields, out                                        AssistantPrepareEventArgs eventContext)
        {
            eventContext = null;
            if (PreSelectItem != null)
            {
                eventContext = new AssistantPrepareEventArgs();
                eventContext.Entity = entity;
                eventContext.Fields = fields;
                PreSelectItem(this, eventContext);

                return !eventContext.Canceled;
            }
            return true;
        }

        private bool AssertContinueUpdating(AssistantEntity entity, string[] fields, out bool stoped)
        {
            stoped = false;
            if (null != PreUpdateItem)
            {
                AssistantPrepareEventArgs args = new AssistantPrepareEventArgs();
                args.Entity = entity;
                args.Fields = fields;

                PreUpdateItem(this, args);
                stoped = args.Stoped;
                return !args.Stoped;
            }
            return true;
        }

        public List<T> SelectList<T>(IConnection conn, string tablename, Criteria condition, Order[] orders, int offset, int count, object[] fields) 
        {
            AssistantPrepareEventArgs args;
            AssistantEntity typedEntity = new AssistantEntity(typeof(T));
            typedEntity.TableName = tablename;
            typedEntity.Criteria = condition;

            AssistantQueryParameter parameter = new AssistantQueryParameter();
            parameter.Orders = orders;
            parameter.Offset = offset;
            parameter.Count = count;

            bool isListField = (null != fields) && (fields.Length > 0) && (fields[0] is ListField);
            List<T> result = new List<T>();
            string[] actualFields = null;
            if (isListField)
            { 
                actualFields = new string[fields.Length];
                for (int i = 0; i < actualFields.Length; i++)
                {
                    actualFields[i] = (fields[i] as ListField).FieldName;
                }
            }
            if (AssertContinueSelecting(typedEntity, parameter, (isListField ? actualFields : (fields as string[])), out args))
            {
                ObjectManager oa = string.IsNullOrEmpty(tablename) ? _d1.GetObjectManager(typeof(T)) : _d1.GetObjectManager(tablename);
                if (isListField)
                {
                    result = oa.MyList<T>(conn, condition, orders, offset, count, (fields as ListField[]));
                }
                else
                {
                    result = oa.MyList<T>(conn, condition, orders, offset, count, (fields as string[]));
                }
            }
            else if (null != args.ContextData &&
                !ReferenceEquals(parameter, args.ContextData))
            { 
                result = (args.ContextData as List<T>) ?? new List<T>();
            }

            return result;
        }

        


    }

    public sealed class AssistantEntity
    {
        public AssistantEntity(Type t)
        {
            this.Type = t;
        }

        public AssistantEntity(object entity)
        {
            this.Entity = entity;
        }

        private object _entity = null;
        public object Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }

        private Type _entityType = null;
        public Type Type
        {
            get { return _entityType ?? (null == Entity ? null : Entity.GetType()); }
            private set { _entityType = value; }
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            internal set { _tableName = value; }
        }

        private Criteria _criteria;
        public Criteria Criteria
        {
            get { return _criteria; }
            internal set { _criteria = value; }
        }
    }

    public sealed class AssistantQueryParameter
    {
        private Order[] orders;
        public Order[] Orders
        {
            get { return orders; }
            internal set { orders = value; }
        }

        private int _offset;
        public int Offset
        {
            get { return _offset; }
            internal set { _offset = value; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            internal set { _count = value; }
        }
    }

    public class AssistantEventArgs : EventArgs
    {
        private AssistantEntity _entity;
        public AssistantEntity Entity
        {
            get { return _entity; }
            internal set { _entity = value; }
        }

        private string[] _fields;
        public string[] Fields
        {
            get { return _fields; }
            internal set { _fields = value; }
        }

        private object _data;
        public object ContextData
        {
            get { return _data; }
            set { _data = value; }
        }
    }

    public sealed class AssistantPrepareEventArgs : AssistantEventArgs
    {
        private bool _canceled = false;
        internal bool Canceled
        {
            get { return _canceled; }
        }

        public void Cancel()
        {
            _canceled = true;
        }

        private bool _stoped;
        public bool Stoped
        {
            get { return _stoped; }
        }

        internal void Stop()
        {
            Cancel();
            _stoped = true;
        }
    }
}
