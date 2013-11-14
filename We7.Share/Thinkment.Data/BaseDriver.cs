using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public abstract class BaseDriver : IDbDriver
    {
        public abstract IConnection CreateConnection(string connectionString);


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
