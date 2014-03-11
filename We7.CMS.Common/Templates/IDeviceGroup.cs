using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    public interface IDeviceGroup
    {
        string Name { get; set; }

        string Title { get; set; }

        int Prior { get; set; }

        bool Initial();

        string TemplatePath { get; set; }
    }
}
