using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;

namespace Thinkment.Data
{
    public class SQLiteDriver : BaseDriver
    {

        public override IConnection CreateConnection(string connectionString)
        {
            IConnectionEx cc = CreateConnection();
            connectionString = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            cc.ConnectionString = connectionString;
            cc.Create = true;
            cc.Driver = this;
            return cc;
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            sql.SqlClause = new Regex(@"\s+month\(+[^\(|\)]+\)+",options).Replace(sql.SqlClause, new MatchEvaluator(ReplaceMonth));
            return sql;
        }

        private string ReplaceMonth(Match match)
        {
            string result = match.Value.ToString();
            result = result.Replace(@"month(", "strftime('%m',");
            return result;
        }

        private IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageSQLiteDbConnection();
            }
            else
            {
                return new SQLiteDbConnection();
            }
        }

        class SQLiteDbConnection : IConnectionEx
        {
            string _c1;
            public string ConnectionString
            {
                get
                {
                    return _c1;
                }
                set
                {
                    _c1 = value;
                }
            }

            bool create;
            public bool Create
            {
                get
                {
                    return create;
                }
                set
                {
                    create = value;
                }
            }

            IDbDriver _d1;
            public IDbDriver Driver
            {
                get
                {
                    return _d1;
                }
                set
                {
                    _d1 = value;
                }
            }

            bool _t1;
            public bool IsTransaction
            {
                get
                {
                    return _t1;
                }
                set
                {
                    _t1 = value;
                }
            }

            SQLiteConnection _c2;
            public SQLiteConnection Connection
            {
                get { return _c2; }
                set { _c2 = value; }
            }

            SQLiteTransaction _t2;
            public SQLiteTransaction Transaction
            {
                get { return _t2; }
                set { _t2 = value; }
            }

            public void Commit()
            {
                if (_t2 != null)
                {
                    _t2.Commit();
                    _t2.Dispose();
                    _t2 = null;
                }
            }

            private SQLiteCommand CreateCommand(SqlStatement sql)
            {
                SQLiteCommand _c = new SQLiteCommand(sql.SqlClause);
                if (Connection == null)
                {
                    _c2 = new SQLiteConnection(_c1);
                    _c2.Open();
                }
                if (IsTransaction)
                {
                    if (_t2 == null)
                    {
                        _t2 = Connection.BeginTransaction();
                    }
                    _c.Transaction = _t2;
                }
                _c.Connection = Connection;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;
                foreach (DataParameter dp in sql.Parameters)
                {
                    SQLiteParameter msp = new SQLiteParameter();
                    msp.ParameterName = dp.ParameterName;
                    msp.Size = dp.Size;
                    msp.Direction = dp.Direction;
                    msp.IsNullable = dp.IsNullable;
                    msp.Value = dp.Value == null ? DBNull.Value : dp.Value;
                    _c.Parameters.Add(msp);
                }
                return _c;
            }

            private void PopulateCommand(SQLiteCommand cmd, SqlStatement sql)
            {
                for (int i = 0; i < sql.Parameters.Count; i++)
                {
                    DataParameter dp = sql.Parameters[i];
                    if (dp.Direction != ParameterDirection.Input)
                    {
                        dp.Value = cmd.Parameters[i].Value;
                    }
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (SQLiteCommand cmd = CreateCommand(sql))
                {
                    object _o = cmd.ExecuteScalar();
                    PopulateCommand(cmd, sql);
                    if (!create)
                        Dispose(true);
                    return _o;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (SQLiteCommand cmd = CreateCommand(sql))
                {
                    cmd.Connection = Connection;
                    if (Connection.State != ConnectionState.Open)
                    {
                        Connection.Open();
                    }
                    int ret = cmd.ExecuteNonQuery();
                    PopulateCommand(cmd, sql);
                    if (!create)
                        Dispose(true);
                    return ret;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    if (_t2 != null)
                    {
                        Commit();
                    }
                    if (_c2 != null)
                    {
                        _c2.Close();
                        _c2.Dispose();
                        _c2 = null;
                    }
                    GC.SuppressFinalize(this);
                }
            }
        }

        class FrontPageSQLiteDbConnection : IConnectionEx
        {

            public string ConnectionString
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool Create
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IDbDriver Driver
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Commit()
            {
                throw new NotImplementedException();
            }

            public bool IsTransaction
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                throw new NotImplementedException();
            }

            public int Update(SqlStatement sql)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
