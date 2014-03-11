using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common;
using System.Web;
using WURFL.Config;
using WURFL;
using We7.Framework;
using We7.Framework.Config;
using System.Reflection;

namespace We7.CMS
{
    public class DefaultTemplateService : ITemplateService
    {
        public static List<IDeviceGroup> deviceGroups = new List<IDeviceGroup>();

        public void Initial()
        {
            string wurflDataFile = HttpContext.Current.Server.MapPath(Constants.WurflDataFilePath);
            string wurflPatchFile = HttpContext.Current.Server.MapPath(Constants.WurflPatchFilePath);

            InMemoryConfigurer configurer = new InMemoryConfigurer().MainFile(wurflDataFile).PatchFile(wurflPatchFile);
            IWURFLManager manager = WURFLManagerBuilder.Build(configurer);
            AppCtx.Cache.AddObject(Constants.WurflManagerCacheKey, manager);

            #region 初始化所有的设备组实现类
            if (deviceGroups == null)
                deviceGroups = new List<IDeviceGroup>();
            if (deviceGroups.Count == 0)
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (string.IsNullOrEmpty(si.DeviceGroups))
                {
                    Exception ex = new Exception("请配置IDeviceGroup设备组接口的实现类，在general.config里配置。");
                    Framework.LogHelper.WriteLog(GetType(), ex);
                    throw ex;
                }
                string[] DeviceGroups = si.DeviceGroups.Split(';');
                foreach (string s in DeviceGroups)
                {
                    Assembly assembly = Assembly.Load(s.Split(',')[1]);
                    string className = s.Split(',')[0];
                    IDeviceGroup gGroup = (IDeviceGroup)assembly.CreateInstance(className, true);
                    gGroup.Initial();
                    deviceGroups.Add(gGroup);
                }
                deviceGroups.Sort(new DeviceGroupComparer());
                deviceGroups.Reverse();
            }

            #endregion
        }
    }

    internal class DeviceGroupComparer : IComparer<IDeviceGroup>
    {

        public int Compare(IDeviceGroup x, IDeviceGroup y)
        {
            return x.Prior - y.Prior;
        }
    }
}
