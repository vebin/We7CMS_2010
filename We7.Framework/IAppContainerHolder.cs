using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Factable;

namespace We7.Framework
{
    public interface IAppContainerHolder
    {
        IContainer ContainerInstance { get;set; }
    }
}
