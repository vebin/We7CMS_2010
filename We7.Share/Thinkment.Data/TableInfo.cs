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

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
