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

        private int _timeOut = 3600;

        protected int TimeOut
        {
            get { return _timeOut > 0 ? _timeOut : 3600; }
            set { _timeOut = value > 0 ? value : 3600; }
        }

        public void AddObject(string objId, object o, int timeOut)
        {
            if (string.IsNullOrEmpty(objId) || string.IsNullOrEmpty(objId.Trim()))
                return;
            CacheItemRemovedCallback callback = new CacheItemRemovedCallback(onRemove);
            if (TimeOut > 0)
            {
                webCache.Insert(objId, o, null, DateTime.Now.AddMilliseconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callback);
            }
            else
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callback);
            }
        }

        public void onRemove(string key, object value, CacheItemRemovedReason reason)
        {
            switch (reason)
            { 
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    break;
                case CacheItemRemovedReason.Removed:
                    break;
                case CacheItemRemovedReason.Underused:
                    break;
                default:
                    break;
            }
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
