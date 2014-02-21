using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using We7.Framework;
using We7.Framework.Factable;

namespace We7.CMS.Web
{
    public class Global : HttpApplication, IAppContainerHolder
    {
        static readonly string ERROR_PAGE_LOCATION = "~/errors.aspx";

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            IWe7CmsStarter starter = new ApplicationStarter();
            starter.Start(this);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

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