using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.Framework.Factable;
using System.Reflection;

namespace We7.CMS.Common.AppFoundation
{
    public class AppLoader : IWe7CmsInitializeModule, ISingletonFactable
    {
        readonly IFactableRegister _factableRegister;
        readonly IAppDescriptorManager _appDescriptorManager;

        public AppLoader(IFactableRegister factableRegister,
            IAppDescriptorManager appDescriptorManager)
        {
            _factableRegister = factableRegister;
            _appDescriptorManager = appDescriptorManager;
        }

        public void InitWe7()
        {
            IEnumerable<AppInfo> apps = _appDescriptorManager.GetApps();
            foreach (AppInfo info in apps)
            {
                if (info.IsEnable)
                {
                    IEnumerable<Assembly> assemblies = _appDescriptorManager.GetAppAssembly(info);
                    foreach (Assembly assembly in assemblies)
                    {
                        _factableRegister.RegisterHandlers(assembly);
                        break;
                    }
                }
            }
        }
    }
}
