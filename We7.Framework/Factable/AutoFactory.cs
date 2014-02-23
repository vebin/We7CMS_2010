using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace We7.Framework.Factable
{
    public interface IFactableRegister
    {
        void RegisterHandlers(Assembly assembly);
    }

    public class AutoFactory : IFactableRegister
    {
        readonly IContainer _container;
        readonly Type _factableType = typeof(IFactable);
        readonly Type _singleFactable = typeof(ISingletonFactable);

        public AutoFactory(IContainer container)
        {
            _container = container;
        }


        public void RegisterHandlers(Assembly assembly)
        {
            if (null == assembly)
                throw new ArgumentNullException("assembly");

            Type enumerable = typeof(IEnumerable);
            Type[] allTypes = assembly.GetTypes();
            List<Type> implements = new List<Type>();

            foreach (Type type in allTypes)
            {
                if (type.IsClass && type.IsPublic
                    && !type.IsAutoClass && !type.IsAbstract
                    && !type.IsGenericTypeDefinition
                    && !enumerable.IsAssignableFrom(type)
                    && _factableType.IsAssignableFrom(type))
                    implements.Add(type);
            }

            foreach (Type implement in implements)
            {
                RegisterClass(implement, null);
            }
        }

        void RegisterClass(Type classType, Type interfaceType)
        { 
            bool firstLevel = (null == interfaceType);
            Type[] interfaces = (firstLevel ? classType : interfaceType).GetInterfaces();

            bool forcedSingletion = false;
            foreach (Type i in interfaces)
            {
                if (i == _singleFactable)
                {
                    forcedSingletion = true;
                    break;
                }
            }
            foreach (Type i in interfaces)
            {
                if ((i != _singleFactable) && (i != _factableType)
                    && _factableType.IsAssignableFrom(i))
                {
                    ComponentRegistration descriptor = new ComponentRegistration(classType);
                    descriptor.ServiceType = i;
                    descriptor.IsSingleton = (forcedSingletion || _singleFactable.IsAssignableFrom(i));
                    try
                    {
                        _container.Register(descriptor);
                    }
                    catch (Exception ex)
                    { 
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(
                            string.Format("无法将类型 '{0}' 注册为 '{1}'，发生了异常：{2}",
                                                    classType.FullName,
                                                    i.FullName,
                                                    ex.Message));
#endif
                    }
                }
                RegisterClass(classType, i);
            }
        }
    }
}
