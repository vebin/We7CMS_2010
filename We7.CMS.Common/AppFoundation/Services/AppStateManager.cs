using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.FileSystem;
using We7.Framework.Cache;
using We7.Framework;
using System.Web.Hosting;
using System.Web.Caching;
using System.IO;

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

        public IEnumerable<string> EnabledAppIds()
        {
            return ReadAppState();
        }

        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
            switch (reason)
            { 
                case CacheItemRemovedReason.DependencyChanged:
                    _appDomainManager.ResetAppDomain();
                    break;
                case CacheItemRemovedReason.Expired:
                    break;
                case CacheItemRemovedReason.Removed:
                    _appDomainManager.ResetAppDomain();
                    break;
                case CacheItemRemovedReason.Underused:
                    break;
            }
        }

        IEnumerable<string> ReadAppState()
        {
            var appstatePath = HostingEnvironment.MapPath(AppStateData);
            List<string> list = _cacheStrategy.RetrieveObject<List<string>>(appstatePath);

            if (list == null)
            {
                list = new List<string>();
                if (File.Exists(appstatePath))
                {
                    var content = File.ReadAllText(appstatePath, Encoding.UTF8);
                    if (!string.IsNullOrEmpty(content))
                    {
                        list.AddRange(content.Split(','));
                    }
                }
                else
                {
                    WriteAppState(null);
                }

                _cacheStrategy.AddObjectWithFileChange(appstatePath, list, onRemove, appstatePath);
            }

            return list;
        }

        void WriteAppState(List<string> appsIds)
        {
            var appstatePath = HostingEnvironment.MapPath(AppStateData);
            File.WriteAllText(appstatePath, appsIds == null ? string.Empty : string.Join(",", appsIds.ToArray()), Encoding.UTF8);
        }
    }
}
