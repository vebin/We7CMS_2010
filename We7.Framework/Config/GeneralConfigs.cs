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

        private static GeneralConfigInfo m_configinfo;

        public static GeneralConfigInfo GetConfig()
        {
            //string configid = "generalconfig";
            GeneralConfigInfo config = null;
            if (config == null)
            {
                config = GeneralConfigFileManager.LoadConfig();
            }

            return config;
        }

        public static void ResetConfig()
        {
            m_configinfo = GeneralConfigFileManager.LoadRealConfig();
        }

        public static bool SaveConfig(GeneralConfigInfo configinfo)
        {
            bool ret = false;
            lock (m_lockHelper)
            {
                ret = SerializationHelper.Save(configinfo, GeneralConfigFileManager.ConfigFilePath);
                ResetConfig();
            }
            return ret;
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
