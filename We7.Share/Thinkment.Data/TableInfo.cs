using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Thinkment.Data
{
    [Serializable]
    public class TableInfo : IParameterExtension<TableInfo>
    {

        public TableInfo(DataTable table, Dictionary<string, Property> pro)
        {
            this._table = table;
            this.FieldsInfo = pro;
        }

        private DataTable _table;
        private Dictionary<string, Property> FieldsInfo;

        private string primaryKeyName = "ID";
        public string PrimaryKeyName
        {
            get { return primaryKeyName; }
            set { primaryKeyName = value; }
        }

        private string id = string.Empty;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private List<FieldsDic> fieldsDic
        {
            get;
            set;
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public string GetFieldValue(string key)
        {
            if (fieldsDic != null)
            {
                FieldsDic field = fieldsDic.Find(delegate(FieldsDic f) { if (string.Compare(key, f.Key, true) == 0) return true; else return false; });
                return field == null ? string.Empty : field.Value;
            }
            else
                return null;
        }
    }

    [Serializable]
    public class FieldsDic
    {
        public FieldsDic(FieldsDic field)
        {
            this.key = field.key;
            this.value = field.value;
        }

        public FieldsDic(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
