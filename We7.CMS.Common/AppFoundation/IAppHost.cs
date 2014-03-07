using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common.AppFoundation
{
    public interface IAppHost : IFactable
    {
        string DirName { get; set; }
    }
}
