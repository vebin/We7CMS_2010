using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace We7.CMS.Common.AppFoundation
{
    public class RequestFilterContext
    {
        internal RequestFilterContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        HttpContext _httpContext;

        public HttpContext HttpContext
        {
            get { return _httpContext; }
            set { _httpContext = value; }
        }

        public HttpApplication ApplicationContext
        { get { return this._httpContext.ApplicationInstance; } }

        public HttpApplicationState Application
        {
            get { return this.HttpContext.Application; }
        }

        public HttpResponse Response { get { return this.HttpContext.Response; } }

        public HttpRequest Request { get { return this.HttpContext.Request; } }

        public HttpServerUtility Server { get { return this.HttpContext.Server; } }

        public HttpSessionState Session { get { return this.HttpContext.Session; } }
    }

    public sealed class ResultExecutingContext : RequestFilterContext
    {
        internal ResultExecutingContext(HttpContext httpContext) : base(httpContext) { }

        public IHttpHandler Handler
        {
            get { return HttpContext.Handler; }
            set { HttpContext.Handler = value; }
        }
    }

    public sealed class ResultExecutedContext : RequestFilterContext
    {
        internal ResultExecutedContext(HttpContext httpContext) : base(httpContext) { }
    }

    public sealed class AuthorizationContext : RequestFilterContext
    { 
        internal AuthorizationContext(HttpContext httpContext) : base(httpContext){}

        bool _success = true;

        public IHttpHandler Handler
        {
            get { return this.HttpContext.Handler; }
            set { this.HttpContext.Handler = value; }
        }

        public void Success()
        {
            _success = true;
        }

        public bool Authorized
        {
            get { return _success; }
        }
    }

    public sealed class ApplicationErrorContext : RequestFilterContext
    {
        internal ApplicationErrorContext(HttpContext httpContext) : base(httpContext) { }

        Exception _exception;

        public Exception ErrorThrown
        {
            get { return _exception; }
            set { _exception = value; }
        }
        bool _handled;

        public bool ErrorHandled
        {
            get { return _handled; }
            set { _handled = value; }
        }
    }
}
