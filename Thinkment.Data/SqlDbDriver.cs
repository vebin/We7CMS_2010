using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public class SqlDbDriver : BaseDriver
    {
        public override IConnection CreateConnection(string connectionString)
        {
            IConnectionEx cc = CreateConnection();
            string cs = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            cc.ConnectionString = cs;
            cc.Driver = this;
            cc.Create = true;
            return cc;
        }

        public IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageSqlDbConnection();
            }
            else
            {
                return new SqlDbConnection();
            }
        }
    }

    class SqlDbConnection : IConnectionEx
    { 
        
    }

    class FrontPageSqlDbConnection : IConnectionEx
    { 
        
    }
}
