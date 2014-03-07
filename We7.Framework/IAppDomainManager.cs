using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.FileSystem;
using System.IO;

namespace We7.Framework
{
    public interface IAppDomainManager : ISingletonFactable
    {
        void ResetAppDomain();
    }

    public class AppDomainManager : IAppDomainManager
    {
        readonly IVirtualPathProvider _virtualPathProvider;

        public AppDomainManager(IVirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider;
        }

        public void ResetAppDomain()
        {
            try
            {
                const string path = "~/bin/ResetAppDomain.txt";
                using (StreamWriter sw = _virtualPathProvider.CreateText(path))
                {
                    sw.WriteLine("此为网站重启文件.勿动");
                    sw.Flush();
                }
            }
            finally { }
        }
    }
}
