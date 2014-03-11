using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    public class TabletDeviceGroup : IDeviceGroup
    {
        int prior = 2;
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
            Prior = GeneralConfigs.GetConfig().TabletTemplateGroupPrior;
            templatePath = GeneralConfigs.GetConfig().TabletTemplateGroupFileName;
            return true;
        }

        string templatePath = GeneralConfigs.GetConfig().TabletTemplateGroupFileName;
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

        string name = "tabletDeviceGroup";
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

        string title = "平板设备组";
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
