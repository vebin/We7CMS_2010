using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    class GeneralConfigFileManager : DefaultConfigFileManager
    {
        private static GeneralConfigInfo m_configinfo;
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (GeneralConfigInfo)value; }
        }

        private static string filename = null;
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        filename = context.Server.MapPath("~/Config/general.config");
                    }
                    else
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (path.ToLower().EndsWith("bin"))
                        {
                            path = path.Substring(0, path.Length - 4);
                        }
                        filename = Path.Combine(path, "Config/general.config");
                    }

                    if (!File.Exists(filename))
                    {
                        filename = "";
                    }
                }
                return filename;
            }
        }

        private static DateTime m_fileoldchange;

        public static GeneralConfigInfo LoadConfig()
        {
            if (ConfigInfo != null)
            {
                m_fileoldchange = File.GetLastWriteTime(ConfigFilePath);
                ConfigInfo = GeneralConfigFileManager.LoadConfig(ref m_fileoldchange, m_configfilepath, m_configinfo, true);
            }
            else
            {
                filename = HttpContext.Current.Server.MapPath("~/Config/general.config");
                if (null == ConfigInfo)
                {
                    ConfigInfo = new GeneralConfigInfo();
                }
                if (!File.Exists(filename))
                {
                    SerializationHelper.Save(ConfigInfo, filename);
                }
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, m_configfilepath, m_configinfo, false);
            }

            return ConfigInfo as GeneralConfigInfo;
        }
    }
}
