using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Factable
{
    public class ComponentRegistration : ICloneable
    {
        public ComponentRegistration(Type type)
        {
            if (null == type)
                throw new ArgumentNullException("componentType");
            _componentType = type;
        }

        public ComponentRegistration(Type serviceType, Type componentType)
        {
            if (null == serviceType)
                throw new ArgumentNullException("serviceType");
            this.ServiceType = serviceType;
            if (null == componentType)
                throw new ArgumentNullException("componentType");
            this.ComponentType = componentType;
        }

        Type _componentType;

        public Type ComponentType
        {
            get { return _componentType; }
            set { _componentType = value; }
        }
        Type _serviceType;

        public Type ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }
        bool _isSingleton;

        public bool IsSingleton
        {
            get { return _isSingleton; }
            set { _isSingleton = value; }
        }
        object _instance;

        public object Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        internal event EventHandler OnRegister;

        public ComponentRegistration Singleton()
        {
            return Singleton(null);
        }

        public void As<TService>() where TService : class
        { 
            ServiceType = typeof(TService);
            if (null != OnRegister)
            {
                OnRegister(this, EventArgs.Empty);
            }
        }

        public ComponentRegistration Singleton(object instance)
        {
            this.IsSingleton = true;
            if (null != instance)
            {
                if (this.ComponentType.IsInstanceOfType(instance))
                {
                    this.IsSingleton = true;
                    this.Instance = instance;
                }
                else
                {
                    this.IsSingleton = false;
                }
            }
            return this;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ComponentRegistration Clone()
        {
            ComponentRegistration instance = new ComponentRegistration(this.ServiceType, this.ComponentType);
            if (this.IsSingleton)
            {
                instance.IsSingleton = this.IsSingleton;
                instance.Instance = this.Instance;
            }
            CloneInstance(instance);
            return instance;
        }

        protected virtual void CloneInstance(ComponentRegistration targetInstance)
        { 
            
        }
    }
}
