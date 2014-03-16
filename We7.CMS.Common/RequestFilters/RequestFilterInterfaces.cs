using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using System.Web.SessionState;
using System.Web;

namespace We7.CMS.Common.AppFoundation
{
    public interface IRequestFilter : IFactable, IRequiresSessionState
    {
    }

    public interface IBeginRequestFilter : IRequestFilter
    {
        void BeginRequest(HttpApplication application);
    }

    public interface IResultFilter : IRequestFilter
    {
        void ResultExecuting(ResultExecutingContext context);

        void ResultExecuted(ResultExecutedContext context);
    }

    public interface IAuthorizationFilter : IRequestFilter
    {
        void OnAuthorize(AuthorizationContext context);
    }

    public interface IReleasingFilter : IRequestFilter
    {
        void SendingHeaders(RequestFilterContext context);

        void SendingContent(RequestFilterContext context);
    }

    public interface IErrorFilter : IRequestFilter
    {
        void Error(ApplicationErrorContext context);
    }
}
