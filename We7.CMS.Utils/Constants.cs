using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS
{
    public static class Constants
    {
        public static string SiteSkinsBasePath
        {
            get
            {
                string _default = GeneralConfigs.GetConfig().SiteSkinsBasePath;
                if (_default == null || _default == string.Empty)
                    _default = "_skins";
                return _default;
            }
        }

        public static string TemplateGroupBasePath
        {
            get { return SiteSkinsBasePath; }
        }

        public static string ThemePath = "theme";

        public const string ArticleModelName = "System.Article";
        
    }
}
