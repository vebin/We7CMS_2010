using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    public class MobileDeviceGroup : IDeviceGroup
    {
        int prior = 1;
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

        public bool Initial()
        {
            Prior = GeneralConfigs.GetConfig().MobileTemplateGroupPrior;
            templatePath = GeneralConfigs.GetConfig().MobileTemplateGroupFileName;
            return true;
        }

        string templatePath = GeneralConfigs.GetConfig().MobileTemplateGroupFileName;
        public string TemplatePath
        {
            get
            {
                return templatePath;
            }
            set
            {
                templatePath = value;
            }
        }

        string name = "mobileDeviceGroup";
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

        string title = "手机设备组";
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
