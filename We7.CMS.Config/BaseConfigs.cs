using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Config
{
    public class BaseConfigs
    {
        private static BaseConfigInfo m_configinfo;

        private static object lockHelper = new object();

        public static bool ConfigFileExist()
        {
            BaseConfigFileManager.filename = "";
            return BaseConfigFileManager.ConfigFilePath != "";
        }

        public static BaseConfigInfo GetBaseConfig()
        {
            if (m_configinfo == null)
            {
                lock (lockHelper)
                {
                    if (m_configinfo == null)
                    {
                        m_configinfo = BaseConfigFileManager.LoadRealConfig();
                    }
                }
            }

            return m_configinfo;
        }

        public static void ResetConfig()
        {
            m_configinfo = BaseConfigFileManager.LoadRealConfig();
        }

        public static bool SaveConfigTo(BaseConfigInfo bci, string configFilePath)
        {
            BaseConfigFileManager bcfm = new BaseConfigFileManager();
            return bcfm.SaveConfig(configFilePath, bci);
        }
    }
}
