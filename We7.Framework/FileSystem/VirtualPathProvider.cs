using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.IO;

namespace We7.Framework.FileSystem
{
    public class VirtualPathProvider : IVirtualPathProvider
    {
        public virtual string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }

        public virtual StreamWriter CreateText(string virtualPath)
        {
            return File.CreateText(MapPath(virtualPath));
        }
    }
}
