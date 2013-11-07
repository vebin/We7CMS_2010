using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace We7.Framework.Config
{
    class SiteConfigFileManager : DefaultConfigFileManager
    {
        private static object m_lockHelper = new object();
        private static SiteConfigInfo m_configinfo;
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (SiteConfigInfo)value; }
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
                        filename = context.Server.MapPath("~/Config/site.config");
                    }
                    else
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (path.ToLower().IndexOf("bin") > -1)
                        {
                            path = path.Substring(0, path.Length - 4);
                        }
                        filename = Path.Combine(path, "Config/site.config");
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

        public static SiteConfigInfo LoadConfig()
        {
            try
            {
                if (ConfigInfo != null)
                {
                    m_fileoldchange = File.GetLastWriteTime(ConfigFilePath);
                    ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
                }
                else 
                {
                    filename = HttpContext.Current.Server.MapPath("~/Config/site.config");
                    ConfigInfo = new SiteConfigInfo();
                    ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, false);
                }
            }
            catch
            {
                
            }

            return ConfigInfo as SiteConfigInfo;
        }

        public static SiteConfigInfo LoadRealConfig()
        {
            if (ConfigFilePath != "")
            {
                lock (m_lockHelper)
                {
                    ConfigInfo = DeserializeInfo(ConfigFilePath, typeof(SiteConfigInfo));
                }
            }
            return ConfigInfo as SiteConfigInfo;
        }
    }
}
