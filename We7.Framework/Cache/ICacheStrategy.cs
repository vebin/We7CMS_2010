using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace We7.Framework.Cache
{
    public interface ICacheStrategy : ISingletonFactable
    {
        void AddObject(string objId, object o, int timeOut);

        void AddObjectWithFileChange(string objId, object o, CacheItemRemovedCallback callback, params string[] files);

        void RemoveObject(string objId);

        object RetrieveObject(string objId);

        T RetrieveObject<T>(string objId);

    }
}
