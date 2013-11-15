using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Data;

namespace Thinkment.Data
{
    public class OracleDriver : BaseDriver
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

        public override string BuildPaging(string table, string fields, string where, List<Order> orders, int from, int count)
        {
            if (orders == null || orders.Count == 0)
                throw new Exception("Order information is required by paging function (OleDbDriver).");
            string ods = BuildOrderString(orders, false);
            string ws = "";
            if (where != null && where.Length > 0)
                ws = " WHERE " + where;
            if (from > 0)
            {
                string rods = BuildOrderString(orders, true);
                string fmt = @"SELECT {2} FROM (SELECT ROWNUM RECNO, {2} FROM (SELECT {2} FROM {3} {4} ORDER BY {5}) AS TB__1 WHERE ROWNUM <= ({0}+{1}) ORDER BY ASC) AS TB__2 WHERE RECNO BETWEEN {1}+1 AND ({0}+{1})";
                return string.Format(fmt, count, from, fields, table, ws, ods);
            }
            else if (count > 0)
            {
                string fmt = @"SELECT {1} FROM (SELECT * FROM {2} {3} ORDER BY {4}) WHERE ROWNUM <= {0}";
                return string.Format(fmt, count, fields, table, ws, ods);
            }
            else
            {
                string fmt = @"SELECT {0} FROM {1} {2} ORDER BY {3}";
                return string.Format(fmt, fields, table, ws, ods);
            }
        }

        public override string FormatTable(string table)
        {
            return string.Format("\"{0}\" ", table) ;
        }

        public override string FormatField(Adorns adorn, string field)
        {
            switch (adorn)
            {
                case Adorns.Average:
                    return string.Format("AVE(\"{0}\") AS \"{0}\"", field);
                case Adorns.Distinct:
                    return string.Format("DISTINCT(\"{0}\") AS \"{0}\"", field);
                case Adorns.Maximum:
                    return string.Format("MAX(\"{0}\") AS \"{0}\"", field);
                case Adorns.Minimum:
                    return string.Format("MIN(\"{0}\") AS \"{0}\"", field);
                case Adorns.Sum:
                    return string.Format("SUM(\"{0}\") AS \"{0}\"", field);
                case Adorns.None:
                case Adorns.SubString:
                    return string.Format("\"{0}\"", field);
                case Adorns.Total:
                    return string.Format("TOTAL(\"{0}\") AS \"{0}\"", field);
                default:
                    return string.Format("\"{0}\"", field);
            }
        }

        public override string FormatField(Adorns adorn, string field, int start, int length)
        {
            switch (adorn)
            {
                case Adorns.SubString:
                    return string.Format("SUBSTR(\"{0}\"," + (start + 1) + "," + length + ")", field);
                case Adorns.Average:
                case Adorns.Distinct:
                case Adorns.Maximum:
                case Adorns.Minimum:
                case Adorns.Sum:
                case Adorns.Total:
                case Adorns.None:
                default:
                    return string.Format("\"{0}\"", field);
            }
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            Regex alterSql = new Regex(@"(alter\s+table|create\s+table|insert\s+into|delete\s+from|select\s+.+\s+from)\s", options);
            if (alterSql.IsMatch(sql.SqlClause))
            {
                sql.SqlClause.Replace("(", " (");
                sql.SqlClause = new Regex(@"\s+varchar[^(\w]+", options).Replace(sql.SqlClause, " VARCHAR2 ");
                sql.SqlClause = new Regex(@"\s+nvarchar[^(\w]+", options).Replace(sql.SqlClause, " VARCHAR2 ");
                sql.SqlClause = new Regex(@"\s+datetime[\W]+", options).Replace(sql.SqlClause, " DATE");
                sql.SqlClause = new Regex(@"\s+text[\W]+", options).Replace(sql.SqlClause, " CLOB");
                sql.SqlClause = new Regex(@"\s+ntext[\W]+", options).Replace(sql.SqlClause, " NCLOB");
                sql.SqlClause = new Regex(@"\s+int[\W]+", options).Replace(sql.SqlClause, " NUMBER ");
                sql.SqlClause = new Regex(@"\s+decimal[^(\w]+", options).Replace(sql.SqlClause, " NUMBER ");
                sql.SqlClause = new Regex(@"\s+bigint[\W]+", options).Replace(sql.SqlClause, " NUMBER ");
                sql.SqlClause = new Regex(@"\s+money[\W]+", options).Replace(sql.SqlClause, " NUMBER(8,2) ");
            }
            sql.SqlClause = new Regex(@"\s+month\(+[^\(|\)]+\)+", options).Replace(sql.SqlClause, new MatchEvaluator(ReplaceMonth));
            sql.SqlClause = sql.SqlClause.Replace("[", "\"").Replace("]", "\"");
            return sql;
        }

        private string ReplaceMonth(Match match)
        {
            string result = match.Value.ToString();
            result = result.Replace("MONTH", "to_char");
            result = result.Replace(")", " \'MM\')");
            return result;
        }

        private IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageOracleDbConnection();
            }
            else
            {
                return new OracleDbConnection();
            }
        }

        class OracleDbConnection : IConnectionEx
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

            OracleTransaction _t2;

            OracleConnection _c2;
            public OracleConnection Connection
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

            OracleCommand CreateCommand(SqlStatement sql)
            {
                OracleCommand _c = new OracleCommand(sql.SqlClause);
                if (Connection == null)
                {
                    _c2 = new OracleConnection(_c1);
                    _c2.Open();
                }
                if (IsTransaction)
                {
                    if (_t2 == null)
                    {
                        _t2 = _c2.BeginTransaction();
                    }
                    _c.Transaction = _t2;
                }
                _c.Connection = _c2;
                _c.CommandType = sql.CommandType;
                _c.CommandTimeout = 300;
                foreach (DataParameter dp in sql.Parameters)
                {
                    OracleParameter op = new OracleParameter();
                    op.ParameterName = dp.ParameterName;
                    op.Size = dp.Size;
                    op.Direction = dp.Direction;
                    if (dp.Value == null)
                        op.Value = DBNull.Value;
                    else
                        op.Value = dp.Value;
                    if (dp.DbType == DbType.DateTime)
                    {
                        if (dp.Value.ToString() == DateTime.MinValue.ToString() || dp.Value == null)
                        {
                            op.Value = OracleDateTime.Null;
                        }
                        else
                        {
                            op.Value = ((DateTime)dp.Value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        op.Size = 64;
                        _c.CommandText.Replace(op.ParameterName, string.Format("to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", op.ParameterName));
                    }
                    else if (dp.DbType == DbType.String)
                    {
                        if (dp.Size > 65500)
                        {
                            op.OracleType = OracleType.Clob;
                            if (dp.Value == null || dp.Value.ToString().Length == 0)
                            {
                                op.Value = DBNull.Value;
                            }
                        }
                        else
                        {
                            if (dp.Value != null)
                            {
                                op.Value = getSubstringByBytes(dp.Value.ToString(), dp.Size);
                            }
                        }
                    }
                    _c.Parameters.Add(op);
                }

                return _c;
            }

            private string getSubstringByBytes(string src, int count)
            {
                byte[] bwrites = Encoding.UTF8.GetBytes(src);
                if (bwrites.Length >= count)
                {
                    return Encoding.UTF8.GetString(bwrites, 0, count);
                }
                else
                    return Encoding.UTF8.GetString(bwrites);
            }

            private void PopulateCommand(OracleCommand cmd, SqlStatement sql)
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
                using (OracleCommand cmd = CreateCommand(sql))
                {
                    object _o = cmd.ExecuteScalar();
                    PopulateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _o;
                }
            }

            public DataTable Query(SqlStatement sql)
            {
                using (OracleCommand _c = CreateCommand(sql))
                {
                    OracleDataAdapter _s = new OracleDataAdapter(_c);
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

            public int Update(SqlStatement sql)
            {
                using (OracleCommand cmd = CreateCommand(sql))
                {
                    cmd.Connection = Connection;
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        Connection.Open();      
                    }
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
        }

        class FrontPageOracleDbConnection : IConnectionEx
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


            public DataTable Query(SqlStatement sql)
            {
                throw new NotImplementedException();
            }
        }
    }
}
