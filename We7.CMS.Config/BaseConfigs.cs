using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Config
{
    public class BaseConfigs
    {
        public static bool ConfigFileExist()
        {
            BaseConfigFileManager.filename = "";
            return BaseConfigFileManager.ConfigFilePath != "";
        }

    }
}
