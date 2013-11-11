using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable()]
    public enum CriteriaType
    {
        None = 0,

        Like,

        MoreThan,

        MoreThanEquals,

        LessThan,

        LessThanEquals,

        Equals,

        NotEquals,

        NotLike,

        IsNull,

        IsNotNull,

        Desc,

        Asc
    }
}
