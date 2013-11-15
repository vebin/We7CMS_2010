using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace We7.CMS.Common.Enum
{
    public enum TypeOfPasswordHashed
    {
        [Description("不用加密")]
        noneEncrypt = 0,

        [Description("cms加密方式")]
        webEncrypt = 1,

        [Description("BBS加密方式")]
        bbsEncrypt = 2
    }
}
