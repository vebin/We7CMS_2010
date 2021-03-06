﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace Thinkment.Data
{
    public class OleDbDriver : BaseDriver
    {

        public override IConnection CreateConnection(string connectionString)
        {
            IConnectionEx conn = CreateConnection();
            connectionString = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            conn.ConnectionString = connectionString;
            conn.Driver = this;
            conn.Create = true;
            return conn;
        }

        public override string FormatField(Adorns adorn, string field)
        {
            switch (adorn)
            {
                case Adorns.Average:
                    return string.Format("AVE([{0}]) AS [{0}]", field);
                case Adorns.Distinct:
                    return string.Format("DISTINCT([{0}]) AS [{0}]", field);
                case Adorns.Maximum:
                    return string.Format("MAX([{0}]) AS [{0}]", field);
                case Adorns.Minimum:
                    return string.Format("MIN([{0}]) AS [{0}]", field);
                case Adorns.Sum:
                    return string.Format("SUM([{0}]) AS [{0}]", field);
                case Adorns.None:
                case Adorns.SubString:
                    return string.Format("[{0}]", field);
                case Adorns.Total:
                    return string.Format("TOTAL([{0}]) AS [{0}]", field);
                default:
                    return string.Format("[{0}]", field);
            }
        }

        public override string FormatField(Adorns adorn, string field, int start, int length)
        {
            switch (adorn)
            {
                case Adorns.SubString:
                    return string.Format("SUBSTRING([{0}]," + (start + 1) + "," + length + ")", field);
                case Adorns.Average:
                case Adorns.Distinct:
                case Adorns.Maximum:
                case Adorns.Minimum:
                case Adorns.Sum:
                case Adorns.Total:
                case Adorns.None:
                default:
                    return string.Format("[{0}]", field);
            }
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            Regex alterSql = new Regex(@"(alter\s+table|create\s+table|insert\s+into|delete\s+from)\s", options);
            if (alterSql.IsMatch(sql.SqlClause))
            {
                sql.SqlClause = new Regex(@"\s+[^\[]?nvarchar", options).Replace(sql.SqlClause, " varchar");
                sql.SqlClause = new Regex(@"\s+[^\[]?text", options).Replace(sql.SqlClause, " Memo");
                sql.SqlClause = new Regex(@"\s+[^\[]?decimal", options).Replace(sql.SqlClause, " Double");
                sql.SqlClause = new Regex(@"\s+[^\[]?bigint", options).Replace(sql.SqlClause, " Long");
            }
            return sql;
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


            public DataTable Query(SqlStatement sql)
            {
                using (OleDbCommand _c = CreateCommand(sql))
                {
                    OleDbDataAdapter _s = new OleDbDataAdapter(_c);
                    DataTable _d = new DataTable();
                    _s.Fill(_d);
                    PopulateCommand(_c, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _d;
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


            public DataTable Query(SqlStatement sql)
            {
                throw new NotImplementedException();
            }
        }
    }
}
