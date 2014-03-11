using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using System.Web;

namespace We7.CMS.Common.AppFoundation
{
    public interface IAppHandlerReflector : ISingletonFactable
    {
        bool IsAppRequest(string requestPath, out IAppHost host);

        IHttpHandler GetHandlerFromApp(string requestPath);
    }
}
