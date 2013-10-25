using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Thinkment.Data
{
    [Serializable]
    public class SqlStatement
    {
        string _s1;
        CommandType _c1;
        List<DataParameter> _p1;

        public string SqlClause
        {
            get { return _s1; }
            set { _s1 = value; }
        }

        public SqlStatement()
        {
            _c1 = CommandType.Text;
            _p1 = new List<DataParameter>();
        }

        public SqlStatement(string sql) : this()
        {
            SqlClause = sql;
        }

        public CommandType CommandType
        {
            get { return _c1; }
            set { _c1 = value; }
        }

        public List<DataParameter> Parameters
        {
            get { return _p1; }
            set { _p1 = value; }
        }
    }
}
