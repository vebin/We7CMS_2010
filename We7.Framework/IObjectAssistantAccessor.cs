using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Factable;
using Thinkment.Data;

namespace We7.Framework
{
    public interface IObjectAssistantAccessor : IFactable
    {

    }

    public class ObjectAssistantAccessor : IObjectAssistantAccessor
    {
        readonly IObjectAssistantAccessorStorage _storage;

        public ObjectAssistantAccessor(IContainer container)
        {
            _storage = container.Resolve<IObjectAssistantAccessorStorage>();
        }
    }

    internal interface IObjectAssistantAccessorStorage
    { 
        
    }

    internal class ObjectAssistantAccessorStorage : IObjectAssistantAccessorStorage
    {
        Dictionary<string, EntityObject> _tableMappings;
        Dictionary<Type, EntityObject> _typeMappings;
        ObjectAssistant _assistant;
    }
}
