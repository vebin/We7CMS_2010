using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common.AppFoundation.Services;

namespace We7.CMS.Common.AppFoundation
{
    public class DefaultAppManager : IAppManager
    {
        readonly IAppStateManager _appStateManager;
        readonly IAppDescriptorManager _appDescriptorManager;

        public DefaultAppManager(
            IAppStateManager appStateManager,
            IAppDescriptorManager appDescriptorManager)
        {
            _appStateManager = appStateManager;
            _appDescriptorManager = appDescriptorManager;
        }

        public AppInfo GetAppInfo(string appId)
        {
            IEnumerable<AppInfo> appInfos = _appDescriptorManager.GetApps();
            foreach (var appInfo in appInfos)
            {
                if (appInfo.Id == appId)
                    return appInfo;
            }
            return null;
        }
    }
}
