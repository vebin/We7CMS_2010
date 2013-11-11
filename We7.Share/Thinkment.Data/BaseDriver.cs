using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public abstract class BaseDriver : IDbDriver
    {
        public abstract IConnection CreateConnection(string connectionString);


        public virtual SqlStatement FormatSQL(SqlStatement sql)
        {
            return sql;
        }


        public virtual string FormatTable(string table)
        {
            return string.Format("[{0}]", table);
        }


        public abstract string FormatField(Adorns adorn, string field, int start, int length);
    }
}
