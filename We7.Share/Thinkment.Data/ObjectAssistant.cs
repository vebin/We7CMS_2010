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

        public event EventHandler<AssistantPrepareEventArgs> PreSelectItem;

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
