﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

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

        public override string FormatTable(string table)
        {
            return string.Format("[{0}] ", table);
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            return base.FormatSQL(sql);
        }

        class SqlDbConnection : IConnectionEx
        {
            SqlTransaction myTransaction;

            string connectiongString;
            public string ConnectionString
            {
                get
                {
                    return connectiongString;
                }
                set
                {
                    connectiongString = value;
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

            IDbDriver idriver;
            public IDbDriver Driver
            {
                get
                {
                    return idriver;
                }
                set
                {
                    idriver = value;
                }
            }

            SqlConnection myConnection;
            public SqlConnection Connection
            {
                get { return myConnection; }
            }

            bool isTransaction;
            public bool IsTransaction
            {
                get { return isTransaction; }
                set { isTransaction = value; }
            }

            public SqlCommand CreateCommand(SqlStatement sql)
            {
                SqlCommand _c = new SqlCommand(sql.SqlClause);
                if (myConnection == null)
                {
                    myConnection = new SqlConnection(ConnectionString);
                    myConnection.Open();
                }
                if (IsTransaction)
                {
                    if (myTransaction == null)
                        myTransaction = myConnection.BeginTransaction();
                    _c.Transaction = myTransaction;
                }
                _c.Connection = myConnection;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;

                foreach (DataParameter dp in sql.Parameters)
                {
                    SqlParameter sp = new SqlParameter();
                    if (dp.Value == null)
                        sp.Value = DBNull.Value;
                    else if (dp.Value.ToString() == DateTime.MinValue.ToString())
                    {
                        sp.Value = DateTime.Now;
                    }
                    else
                    {
                        sp.Value = dp.Value;
                    }

                    if (dp.DbType == DbType.DateTime && dp.Value != null)
                    {
                        if (Convert.ToDateTime(dp.Value) <= DateTime.Parse("1/1/1753 12:00:00") ||
                            Convert.ToDateTime(dp.Value) >= DateTime.Parse("12/31/9999 11:59:59"))
                        {
                            sp.Value = DBNull.Value;
                        }
                    }
                    sp.ParameterName = dp.ParameterName;
                    sp.Size = dp.Size;
                    sp.Direction = dp.Direction;
                    _c.Parameters.Add(sp);
                }

                return _c;
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (SqlCommand _c = this.CreateCommand(sql))
                {
                    object _o = _c.ExecuteScalar();
                    PopulateCommand(_c, sql);

                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _o;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (SqlCommand cmd = CreateCommand(sql))
                {
                    cmd.Connection = Connection;
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    int ret = cmd.ExecuteNonQuery();
                    PopulateCommand(cmd, sql);

                    if (!create)
                    {
                        Dispose(true);
                    }

                    return ret;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    if (myTransaction != null)
                    {
                        Commit();
                    }
                    if (myConnection != null)
                    {
                        myConnection.Close();
                        myConnection.Dispose();
                        myConnection = null;
                    }
                    GC.SuppressFinalize(this);
                }
            }

            public void Commit()
            {
                if (myTransaction != null)
                {
                    myTransaction.Commit();
                    myTransaction.Dispose();
                    myTransaction = null;
                }
            }

            private void PopulateCommand(SqlCommand cmd, SqlStatement sql)
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

            public DataTable Query(SqlStatement sql)
            {
                using (SqlCommand _c = CreateCommand(sql))
                {
                    SqlDataAdapter _s = new SqlDataAdapter(_c);
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

        class FrontPageSqlDbConnection : IConnectionEx
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
                    return string.Format("SUBSTRING([{0}],"+(start+1)+","+length+")", field);
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
    }
}
