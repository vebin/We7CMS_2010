using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public enum ErrorCodes
    {
        Success = 0,
        UnkownCriteria = 1,

        DriverRequired = 10,
        DriverFailed = 11,

        UnkownObject = 20,
        UnkownProperty = 21,
        ConditionRequired = 22,
        UnmatchType = 23,

        NoPrimaryKey = 34
    }
}
