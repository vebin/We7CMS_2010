using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.FileSystem;

namespace We7.Framework
{
    public interface IAppDomainManager : ISingletonFactable
    {
    }

    public class AppDomainManager : IAppDomainManager
    {
        readonly IVirtualPathProvider _virtualPathProvider;

        public AppDomainManager(IVirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider;
        }
    }
}
