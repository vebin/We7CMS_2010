using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Config
{
    public class GeneralConfigs
    {
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
    }
}
