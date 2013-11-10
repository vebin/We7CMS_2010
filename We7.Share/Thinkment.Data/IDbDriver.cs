using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public interface IDbDriver
    {
        IConnection CreateConnection(string connectionString);

        SqlStatement FormatSQL(SqlStatement sql);

        string FormatTable(string table);
    }
}
