using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.FileSystem;
using We7.Framework.Cache;
using We7.Framework;

namespace We7.CMS.Common.AppFoundation.Services
{
    public class AppStateManager : IAppStateManager
    {
        internal const string AppStateData = "~/App_Code/AppState.dat";
        readonly IEnumerable<IAppHost> _appHosts;
        readonly IVirtualPathProvider _virtualPathProvider;
        readonly ICacheStrategy _cacheStrategy;
        readonly IAppDomainManager _appDomainManager;

        public AppStateManager(IEnumerable<IAppHost> appHosts,
            IVirtualPathProvider virtualPathProvider,
            ICacheStrategy cacheStrategy,
            IAppDomainManager appDomainManager)
        {
            _virtualPathProvider = virtualPathProvider;
            _appHosts = appHosts;
            _cacheStrategy = cacheStrategy;
            _appDomainManager = appDomainManager;
        }
    }
}
