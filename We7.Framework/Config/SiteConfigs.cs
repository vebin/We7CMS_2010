using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    public class SiteConfigs
    {
        private static object m_lockHelper = new object();

        public static SiteConfigInfo GetConfig()
        {
            SiteConfigInfo config = null;
            if (config == null)
            {
                config = SiteConfigFileManager.LoadConfig();
            }
            return config;
        }

        public static SiteConfigInfo Serialize(SiteConfigInfo configinfo, string path)
        {
            lock (m_lockHelper)
            {
                SerializationHelper.Save(configinfo, path);
            }
            return configinfo;
        }

        public static SiteConfigInfo Deserialize(string configFilePath)
        {
            lock (m_lockHelper)
            {
                return (SiteConfigInfo)SerializationHelper.Load(configFilePath, typeof(SiteConfigInfo));
            }
        }
    }
}
