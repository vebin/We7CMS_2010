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
}
