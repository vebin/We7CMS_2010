using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Factable
{
    public static class ContainerContext
    {
        static IAppContainerHolder _holder;

        internal static void SetContainer(IContainer container, IAppContainerHolder holder)
        {
            if (null == holder)
                throw new ArgumentNullException("holder");
            _holder = holder;
            _holder.ContainerInstance = container;

            Register<Container>().Singleton(container).As<IContainer>();
            IContainer con = container.Resolve<IContainer>();
            if (null == con)
                throw new InvalidOperationException("无法初始化系统，无法加载系统所需的类型主容器");
            _holder.ContainerInstance = container;
        }

        public static ComponentRegistration Register<TComponent>() where TComponent : class
        {
            return GetContainerInternal().Register<TComponent>();
        }

        static IContainer GetContainerInternal()
        {
            ThrowIfNoContainer();
            return _holder.ContainerInstance;
        }

        static void ThrowIfNoContainer()
        {
            if (null == _holder)
                throw new InvalidOperationException("无法提供服务，因为没有设置正确的Container");
        }
    }
}
