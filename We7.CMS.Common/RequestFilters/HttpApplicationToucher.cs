using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using System.Web;
using We7.Framework.Factable;
using We7.CMS.Common.AppFoundation;

namespace We7.CMS.Common.RequestFilters
{
    public class HttpApplicationToucher : IObjectToucher<HttpApplication>
    {
        readonly IContainer _container;
        readonly IAppHandlerReflector _appReflector;

        public HttpApplicationToucher(IContainer container, IAppHandlerReflector appReflector)
        {
            _container = container;
            _appReflector = appReflector;
        }

        public void Touch(HttpApplication app)
        {
            app.BeginRequest += application_BeginRequest;
            app.PostResolveRequestCache += application_ReMapHandler;
        }

        void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            InvokeFilters(delegate(IBeginRequestFilter filter)
                                {
                                    filter.BeginRequest(app);
                                });
        }

        void application_ReMapHandler(object sender, EventArgs e)
        {
            HttpContext httpContext = ((HttpApplication)sender).Context;
            string requestPath = httpContext.Request.Path;

            IAppHost matchedHost;
            if (_appReflector.IsAppRequest(requestPath, out matchedHost))
            {
                IHttpHandler handler = _appReflector.GetHandlerFromApp(requestPath);
                if (null != handler)
                    httpContext.RemapHandler(handler);
            }
        }

        void InvokeFilters<TFilter>(Action<TFilter> action) where TFilter : class
        {
            InvokeFilters(action, null);
        }

        void InvokeFilters<TFilter>(Action<TFilter> action, Func<bool> specifyBreak) where TFilter : class
        {
            IEnumerable<TFilter> filters = _container.ResolveAll<TFilter>();
            foreach (TFilter filter in filters)
            {
                try
                {
                    action.Invoke(filter);
                }
                catch (Exception ex)
                { 
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(
                        string.Format("在请求处理过程中，插件类 {0} 的方法调用失败，异常：{1}",
                        filter.GetType().FullName,
                        ex.Message));
#endif
                }
                if (null != specifyBreak && specifyBreak.Invoke())
                {
                    break;
                }
            }
        }
    }
}
