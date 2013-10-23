using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    public class GeneralConfigs
    {
        private static object m_lockHelper = new object();

        public static GeneralConfigInfo GetConfig()
        {
            //string configid = "generalconfig";
            GeneralConfigInfo config = null;
            if (config == null)
            {
                GeneralConfigFileManager.LoadConfig();
            }

            return config;
        }

        public static GeneralConfigInfo Serialize(GeneralConfigInfo configinfo, string configFilePath)
        {
            lock (m_lockHelper)
            {
                SerializationHelper.Save(configinfo, configFilePath);
            }
            return configinfo;
        }

        public static GeneralConfigInfo Deserialize(string configFilePath)
        {
            lock (m_lockHelper)
            {
                return (GeneralConfigInfo)SerializationHelper.Load(configFilePath, typeof(GeneralConfigInfo));
            }
        }
    }
}
