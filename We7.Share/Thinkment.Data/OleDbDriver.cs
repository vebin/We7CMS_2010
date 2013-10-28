using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Thinkment.Data
{
    public class OleDbDriver : BaseDriver
    {

        public override IConnection CreateConnection(string connectionString)
        {
            IConnectionEx conn = CreateConnection();
            conn.ConnectionString = connectionString;
            conn.Driver = this;
            conn.Create = true;
            return conn;
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            return base.FormatSQL(sql);
        }

        private IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageOleConnection();
            }
            else
            {
                return new OleConnection();
            }
        }

        class OleConnection : IConnectionEx
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

            OleDbTransaction _t2;

            OleDbConnection _c2;
            public OleDbConnection Connection
            {
                get { return _c2; }
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

            OleDbCommand CreateCommand(SqlStatement sql)
            {
                OleDbCommand cmd = new OleDbCommand(sql.SqlClause);
                if (Connection == null)
                {
                    try
                    {
                        _c2 = new OleDbConnection(_c1);
                        _c2.Open();
                    }
                    catch
                    {
                    }
                    if (IsTransaction && _t2 == null)
                    {
                        _t2 = _c2.BeginTransaction();
                    }
                }
                if (IsTransaction)
                {
                    if (_t2 == null)
                    {
                        _t2 = _c2.BeginTransaction();
                    }
                    cmd.Transaction = _t2;
                }
                cmd.Connection = _c2;
                cmd.CommandTimeout = 300;
                cmd.CommandType = sql.CommandType;
                foreach (DataParameter dp in sql.Parameters)
                {
                    OleDbParameter odp = new OleDbParameter();
                    if (dp.DbType == DbType.DateTime)
                        odp.OleDbType = OleDbType.Date;
                    else
                        odp.DbType = dp.DbType;
                    odp.ParameterName = dp.ParameterName;
                    odp.Size = dp.Size;
                    odp.Direction = dp.Direction;
                    odp.IsNullable = dp.IsNullable;
                    odp.Value = dp.Value == null ? DBNull.Value : dp.Value;
                    cmd.Parameters.Add(odp);
                }
                return cmd;
            }

            private void PopulateCommand(OleDbCommand cmd, SqlStatement sql)
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
                using (OleDbCommand cmd = CreateCommand(sql))
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
                using (OleDbCommand cmd = CreateCommand(sql))
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

        class FrontPageOleConnection : IConnectionEx
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
