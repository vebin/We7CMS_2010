using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Factable
{
    public interface IContainer
    {
        ComponentRegistration Register<TComponent>() where TComponent : class;

        TService Resolve<TService>() where TService : class;

        IEnumerable<TService> ResolveAll<TService>() where TService : class;
    }
}
