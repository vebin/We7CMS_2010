using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public abstract class BaseDriver : IDbDriver
    {
        public abstract IConnection CreateConnection(string connectionString);
    }
}
