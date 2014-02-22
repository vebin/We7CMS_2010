using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Factable
{
    public interface IContainerStorage
    {
        void Store(ComponentRegistration descriptor);

        ComponentRegistration Fetch(Type tService);

        IEnumerable<ComponentRegistration> FetchAll(Type tService);
    }

    internal sealed class ContainerStorage : IContainerStorage
    {
        Dictionary<Type, object> _instanceList = new Dictionary<Type, object>();
        Dictionary<Type, List<ComponentRegistration>> _registrationList = new Dictionary<Type, List<ComponentRegistration>>();
        static object _locker = new object();

        public void Store(ComponentRegistration descriptor)
        {
            if (null == descriptor)
                return;
            List<ComponentRegistration> stored;
            if (_registrationList.TryGetValue(descriptor.ServiceType, out stored))
            {
                int index = stored.FindIndex(delegate(ComponentRegistration item) 
                                            {
                                                return item.ComponentType == descriptor.ComponentType;
                                            });
                if (index > -1)
                {
                    _registrationList[descriptor.ServiceType][index] = descriptor;
                }
                else
                {
                    _registrationList[descriptor.ServiceType].Add(descriptor);
                }
            }
            else
            {
                List<ComponentRegistration> newListItem = new List<ComponentRegistration>();
                newListItem.Add(descriptor);
                lock (_locker)
                {
                    _registrationList.Add(descriptor.ServiceType, newListItem);
                }
            }

        }


        public ComponentRegistration Fetch(Type tService)
        {
            IList<ComponentRegistration> all = Internal_FetchAll(tService);
            if (null != all)
            {
                int count = all.Count;
                return all[count - 1];
            }
            return null;
        }

        List<ComponentRegistration> Internal_FetchAll(Type tService)
        {
            List<ComponentRegistration> stored;
            List<ComponentRegistration> ret;
            bool success = false;
            lock (_locker)
            {
                success = _registrationList.TryGetValue(tService, out stored);
            }

            if (success)
            {
                ret = new List<ComponentRegistration>(stored.Count);
                foreach (ComponentRegistration registration in stored)
                {
                    ret.Add(registration.Clone());
                }
                return ret;
            }

            return null;
        }


        public IEnumerable<ComponentRegistration> FetchAll(Type tService)
        {
            List<ComponentRegistration> stored = Internal_FetchAll(tService);

            if (null != stored)
            {
                return stored.AsReadOnly();
            }
            return new ComponentRegistration[0];
        }
    }
}
