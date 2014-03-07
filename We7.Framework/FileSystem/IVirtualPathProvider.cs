using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace We7.Framework.FileSystem
{
    public interface IVirtualPathProvider : IFactable
    {
        StreamWriter CreateText(string virtualPath);

        string MapPath(string virtualPath);
    }
}
