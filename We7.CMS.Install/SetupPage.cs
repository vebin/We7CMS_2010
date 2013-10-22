using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using We7.Framework.Config;
using System.Diagnostics;
using System.Reflection;

namespace We7.CMS.Install
{
    public class SetupPage : System.Web.UI.Page
    {
        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        public static string header = "";

        public static bool LockFileExist()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;
            if (context != null)
            {
                physicsPath = context.Server.MapPath("/config");
            }
            else
            {
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            return File.Exists(physicsPath + "\\db-is-creating.lock");
        }

        public static void Init()
        {
            header = "<HEAD><title>安装"+GetAssemblyProductName()+"</title><meta http-equiv=\"Content-Type\" content=\"text/html\" charset=\"utf-8\">\r\n";
            header += "<LINK rev=\"stylesheet\" media=\"all\" href=\"css.general.css\" type=\"text/css\" rel=\"stylesheet\">\r\n";
            header += "<script type=\"text/javascript\" src=\"js/setup.js\"></script>\r\n";
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();

            if (gi != null)
            {
                string copyright = gi.CopyrightOfWe7;
                if (gi.IsOEM)
                    copyright = gi.Copyright;
            }
        }

        public static string GetAssemblyProductName()
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            if (gi != null)
            {
                return gi.ProductName;
            }
            else
            {
                return AssemblyFileVersion.ProductName;
            }
        }
    }
}
