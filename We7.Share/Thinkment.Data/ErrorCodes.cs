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

        UnkownObject = 20,
        UnkownProperty = 21,
    }
}
