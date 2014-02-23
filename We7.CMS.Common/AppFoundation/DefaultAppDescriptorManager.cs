using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using We7.CMS.Common.AppFoundation.Services;
using We7.Framework.FileSystem;
using We7.Framework;


namespace We7.CMS.Common.AppFoundation
{
    public class DefaultAppDescriptorManager : IAppDescriptorManager
    {
        readonly Dictionary<AppInfo, IEnumerable<Assembly>> _appInfoDic;
        readonly Dictionary<string, Assembly> _appReferencedAssemblies;
        readonly Dictionary<string, AppInfo> _memoryAppInfo;
        readonly object _resolveAssemblylocker = new object();

        readonly IEnumerable<IAppHost> _appHosts;
        readonly IAppStateManager _appStateManager;
        readonly IVirtualPathProvider _virtualPathProvider;

        public DefaultAppDescriptorManager(IEnumerable<IAppHost> appHosts,
            IAppStateManager appStateManager,
            IVirtualPathProvider virtualPathProvider)
        {
            _appHosts = appHosts;
            _appStateManager = appStateManager;
            _virtualPathProvider = virtualPathProvider;

            _appInfoDic = new Dictionary<AppInfo, IEnumerable<Assembly>>();
            _appReferencedAssemblies = new Dictionary<string, Assembly>();
            _memoryAppInfo = new Dictionary<string, AppInfo>();
            //InitAppInfo();
        }

        public IEnumerable<AppInfo> GetApps()
        {
            return _appInfoDic.Keys;
        }

        public IEnumerable<Assembly> GetAppAssembly(AppInfo appInfo)
        {
            return (null == appInfo || _appInfoDic.ContainsKey(appInfo))
                ? Enumerable.Empty<Assembly>() : _appInfoDic[appInfo];
        }
    }
}
