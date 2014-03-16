using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using We7.Framework;
using We7.Framework.Factable;
using We7.Framework.Config;

namespace We7.CMS.Web
{
    public class Global : HttpApplication, IAppContainerHolder
    {
        static readonly string ERROR_PAGE_LOCATION = "~/errors.aspx";

        public override void Init()
        {
            base.Init();
            if (null != ContainerInstance)
            {
                IEnumerable<IObjectToucher<HttpApplication>> touchers =
                    ContainerInstance.ResolveAll<IObjectToucher<HttpApplication>>();
                foreach (IObjectToucher<HttpApplication> toucher in touchers)
                {
                    toucher.Touch(this);
                }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            IWe7CmsStarter starter = new ApplicationStarter();
            starter.Start(this);

            ApplicationHelper.ResetApplication();
            #region 初始化模板服务接口实现类的Initial方法，缓存相关数据
            ITemplateService templateService = TemplateServiceFactory.Create();
            templateService.Initial();
            #endregion
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            if (Application[PageVisitorHelper.OnlinePeopleApplicationKey] != null)
                Application[PageVisitorHelper.OnlinePeopleApplicationKey] = (int)Application[PageVisitorHelper.OnlinePeopleApplicationKey] + 1;
            Application.UnLock();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            We7.Framework.LogHelper.WriteLog(typeof(Global), error);

            if (Context != null && Context.IsCustomErrorEnabled)
                Server.Transfer(ERROR_PAGE_LOCATION, false);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application[PageVisitorHelper.OnlinePeopleApplicationKey] = (int)Application[PageVisitorHelper.OnlinePeopleApplicationKey] - 1;
            Application.UnLock();
            if (GeneralConfigs.GetConfig().StartPageViewModule)
                PageVisitorHelper.PageVisitorLeave();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }

        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        #region IAppContainerHolder 成员
        static IContainer _container;
        public IContainer ContainerInstance
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }
        #endregion
    }
}