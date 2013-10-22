using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    public class DefaultConfigFileManager
    {
        private static object m_lockHelper = new object();

        public static string m_configfilepath;
        public static string ConfigFilePath
        {
            get { return m_configfilepath; }
            set { m_configfilepath = value; }
        }

        private static IConfigInfo m_configinfo = null;
        public static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = value; }
        }

        protected static IConfigInfo LoadConfig(ref DateTime fileoldchange, string configFilePath, IConfigInfo configinfo, bool checkTime)
        {
            lock (m_lockHelper)
            {
                m_configfilepath = configFilePath;
                m_configinfo = configinfo;
                if (checkTime)
                {
                    DateTime filenewchange = File.GetLastWriteTime(m_configfilepath);
                    if (filenewchange != fileoldchange)
                    {
                        fileoldchange = filenewchange;
                        m_configinfo = DeserializeInfo(configFilePath, configinfo.GetType());
                    }
                }
                else
                {
                    m_configinfo = DeserializeInfo(configFilePath, configinfo.GetType());
                }

                return m_configinfo;
            }
        }

        public static IConfigInfo DeserializeInfo(string configFilePath, Type configType)
        {
            return (IConfigInfo)SerializationHelper.Load(configFilePath, configType);
        }
    }
}
