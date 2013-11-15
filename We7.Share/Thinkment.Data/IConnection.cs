using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Thinkment.Data
{
    public interface IConnection : IDisposable
    {
        IDbDriver Driver { get; set; }

        void Commit();

        bool IsTransaction { get; set; }

        DataTable Query(SqlStatement sql);

        object QueryScalar(SqlStatement sql);

        int Update(SqlStatement sql);
    }

    public interface IConnectionEx : IConnection
    {
        string ConnectionString { get; set; }
        bool Create { get; set; }
    }
}
