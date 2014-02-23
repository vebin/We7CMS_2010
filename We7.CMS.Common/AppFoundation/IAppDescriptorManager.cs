using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common.AppFoundation;
using System.Reflection;

namespace We7.CMS.Common.AppFoundation
{
    public interface IAppDescriptorManager : ISingletonFactable
    {
        IEnumerable<AppInfo> GetApps();

        IEnumerable<Assembly> GetAppAssembly(AppInfo appInfo);
    }
}
