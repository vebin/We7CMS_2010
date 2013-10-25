using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public interface IConnection : IDisposable
    {
        IDbDriver Driver { get; set; }

        void Commit();

        object QueryScalar(SqlStatement sql);

        int Update(SqlStatement sql);
    }

    public interface IConnectionEx : IConnection
    {
        string ConnectionString { get; set; }
        bool Create { get; set; }
    }
}
