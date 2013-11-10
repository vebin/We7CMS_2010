using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using We7.CMS.Config;
using We7.Framework.Config;
using Thinkment.Data;
using System.Diagnostics;
using We7.Framework;

namespace We7.CMS
{
    public class ApplicationHelper
    {
        static object lockHelper = new object();
        static object lockHelper2 = new object();

        public static void ResetApplication()
        {
            lock (lockHelper)
            {
                HttpContext context = HttpContext.Current;
                context.Application.Clear();
                if (context.Session != null)
                    context.Session.Clear();
                
                BaseConfigs.ResetConfig();
                SiteConfigs.ResetConfig();
                GeneralConfigs.ResetConfig();
                context.Application["We7.Application.OnlinePeople.Key"] = 0;
                if (BaseConfigs.ConfigFileExist())
                {
                    BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();
                    string root = context.Server.MapPath("~/");
                    string dataPath = context.Server.MapPath("~/App_Data/XML");
                    ObjectAssistant assistant = new ObjectAssistant();
                    try
                    {
                        if (baseconfig != null && baseconfig.DBConnectionString != "")
                        {
                            baseconfig.DBConnectionString = baseconfig.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory).Replace("\\\\", "\\");
                            assistant.LoadDBConnectionString(baseconfig.DBConnectionString, baseconfig.DBDriver);
                        }
                        assistant.LoadDataSource(dataPath);
                    }
                    catch (DataException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        string s = string.Format("注意检查：/app_data/xml目录下的配置文件：\r\n错误代码：{0}\r\n错误消息：{1}",
                                    ex.Errorcode, ex.Message);
                        LogHelper.WriteLog(typeof(ApplicationHelper), s);
                        throw ex;
                    }

                    HelperFactory factory = new HelperFactory();
                    factory.Assistant = assistant;
                    factory.Root = root;
                    factory.Initialize();
                    context.Application["We7.HelperFactory"] = factory;
                }
            }
        }
    }
}
