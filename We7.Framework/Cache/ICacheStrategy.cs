using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework.Cache
{
    public interface ICacheStrategy : ISingletonFactable
    {
        void AddObject(string objId, object o, int timeOut);

        void RemoveObject(string objId);

        object RetrieveObject(string objId);

        T RetrieveObject<T>(string objId);

    }
}
