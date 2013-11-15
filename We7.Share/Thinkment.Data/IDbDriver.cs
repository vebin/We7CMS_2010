using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public interface IDbDriver
    {
        string Prefix { get; }

        string BuildPaging(string tablename, string fields, string where, List<Order> orders, int from, int count);

        IConnection CreateConnection(string connectionString);

        SqlStatement FormatSQL(SqlStatement sql);

        string FormatTable(string table);

        string FormatField(Adorns adorn, string field, int start, int length);

        string FormatField(Adorns adorn, string field);

        string GetCriteria(CriteriaType type);
    }
}
