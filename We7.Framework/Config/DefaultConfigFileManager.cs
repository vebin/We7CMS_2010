using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Config
{
    class DefaultConfigFileManager
    {
        public static string m_configfilepath;

        public static string ConfigFilePath
        {
            get { return m_configfilepath; }
            set { m_configfilepath = value; }
        }
    }
}
