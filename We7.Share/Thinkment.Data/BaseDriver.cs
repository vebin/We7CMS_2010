using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public abstract class BaseDriver : IDbDriver
    {
        public abstract IConnection CreateConnection(string connectionString);


        public virtual string BuildPaging(string table, string fields, string where, List<Order> orders, int from, int count)
        {
            if (orders == null || orders.Count == 0)
                throw new Exception("Order information is required by paging function (OleDbDriver).");
            string ods = BuildOrderString(orders, false);
            string ws = "";
            if (where != null && where.Length > 0)
                ws = " WHERE " + where;
            if (from > 0)
            {
                string rods = BuildOrderString(orders, true, true);
                string rrods = BuildOrderString(orders, false, true);
                string fmt = "SELECT TOP {0} * FROM (SELECT TOP {0} * FROM (SELECT TOP {1} {2} FROM {3} AS TB__1 {4} ORDER BY {5}) AS TB__2 ORDER BY {6}) AS TB__3 ORDER BY {7} ";
                return string.Format(fmt, count, from+count, fields, table, ws, ods, rods, rrods);
            }
            else if (count > 0)
            {
                string fmt = "SELECT TOP {0} {1} FROM {2} {3} ORDER BY {4}";
                return string.Format(fmt, count, fields, table, ws, ods);
            }
            else
            {
                string fmt = "SELECT {0} from {1} {2} ORDER BY {3}";
                return string.Format(fmt, fields, table, ws, ods);
            }
        }

        protected string BuildOrderString(List<Order> orders, bool reverse)
        {
            return BuildOrderString(orders, reverse, false);
        }

        protected string BuildOrderString(List<Order> orders, bool reverse, bool isalias)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Order order in orders)
            {
                if (sb.Length > 0)
                    sb.Append(",");
                if (order.Adorn == Adorns.None)
                    sb.Append(FormatField(Adorns.None, order.Name));
                else
                {
                    if (isalias)
                    {
                        string name = FormatField(order.Adorn, order.AliasName, order.Start, order.Length);
                        sb.Append(name);
                    }
                    else
                    {
                        string name = FormatField(order.Adorn, order.Name, order.Start, order.Length);
                        sb.Append(name);
                    }
                }
                if (order.Mode == OrderMode.Asc && !reverse ||
                    order.Mode == OrderMode.Desc && reverse)
                {
                    sb.Append(" " + GetCriteria(CriteriaType.Asc) + " ");
                }
                else
                {
                    sb.Append(" "+GetCriteria(CriteriaType.Desc)+" ");
                }
            }

            return sb.ToString();
        }

        public virtual SqlStatement FormatSQL(SqlStatement sql)
        {
            return sql;
        }


        public virtual string FormatTable(string table)
        {
            return string.Format("[{0}]", table);
        }


        public abstract string FormatField(Adorns adorn, string field, int start, int length);

        public abstract string FormatField(Adorns adorn, string field);

        public string GetCriteria(CriteriaType type)
        {
            switch (type)
            { 
                case CriteriaType.NotEquals:
                    return "<>";
                case CriteriaType.LessThan:
                    return "<";
                case CriteriaType.LessThanEquals:
                    return "<=";
                case CriteriaType.MoreThan:
                    return ">";
                case CriteriaType.MoreThanEquals:
                    return ">=";
                case CriteriaType.NotLike:
                    return "Not Like";
                case CriteriaType.Like:
                    return "Like";
                case CriteriaType.Equals:
                    return "=";
                case CriteriaType.Desc:
                    return "Desc";
                case CriteriaType.Asc:
                    return "Asc";
                default:
                    throw new DataException(ErrorCodes.UnkownCriteria);

            }
        }

        public string Prefix
        {
            get { return "@"; }
        }
    }
}
