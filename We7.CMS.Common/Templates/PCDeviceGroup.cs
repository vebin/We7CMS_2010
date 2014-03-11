using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    public class PCDeviceGroup : IDeviceGroup
    {
        string templatePath = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
        public string TemplatePath
        {
            get { return templatePath; }
            set { templatePath = value; }
        }

        public bool Initial()
        {
            Prior = GeneralConfigs.GetConfig().DefaultTemplateGroupPrior;
            templatePath = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
            return true;
        }

        int prior = 0;
        public int Prior
        {
            get
            {
                return prior;
            }
            set
            {
                prior = value;
            }
        }

        string name = "pcDeviceGroup";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string title = "PC设备组";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
    }
}
