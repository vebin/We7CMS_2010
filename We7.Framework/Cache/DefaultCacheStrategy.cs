using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace We7.Framework.Cache
{
    public class DefaultCacheStrategy : ICacheStrategy
    {

        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        public void AddObject(string objId, object o, int timeOut)
        {
            if (string.IsNullOrEmpty(objId) || string.IsNullOrEmpty(objId.Trim()))
                return;
            CacheItemRemovedCallback callback = new CacheItemRemovedCallback(onRemove);
        }

        public virtual void RemoveObject(string objId)
        {
            if (string.IsNullOrEmpty(objId))
                return;
            webCache.Remove(objId);
        }

        public virtual T RetrieveObject<T>(string objId)
        {
            object o = RetrieveObject(objId);
            return o != null ? (T)o : default(T);
        }

        public virtual object RetrieveObject(string objId)
        {
            if (string.IsNullOrEmpty(objId))
            {
                return null;
            }
            return webCache.Get(objId);
        }

    }
}
