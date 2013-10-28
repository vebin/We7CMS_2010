using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Thinkment.Data
{
    public class DataParameter : IDbDataParameter
    {
        byte precision;
        public byte Precision
        {
            get
            {
                return precision;
            }
            set
            {
                precision = value;
            }
        }

        byte scale;
        public byte Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        int size;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        DbType dbType;
        public DbType DbType
        {
            get
            {
                return dbType;
            }
            set
            {
                dbType = value;
            }
        }

        ParameterDirection direction;
        public ParameterDirection Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        bool isNullable;
        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        string parameterName;
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }

        string sourceColumn;
        public string SourceColumn
        {
            get
            {
                return sourceColumn;
            }
            set
            {
                sourceColumn = value;
            }
        }

        DataRowVersion sourceVersion;
        public DataRowVersion SourceVersion
        {
            get
            {
                return sourceVersion;
            }
            set
            {
                sourceVersion = value;
            }
        }

        object oValue;
        public object Value
        {
            get
            {
                return oValue;
            }
            set
            {
                oValue = value;
            }
        }
    }
}
