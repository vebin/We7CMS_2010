using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Cache;

namespace We7.Framework
{
    public static class AppCtx
    {
        private static ICacheStrategy cache;

        public static ICacheStrategy Cache
        {
            get 
            {
                if (cache == null)
                {
                    cache = new DefaultCacheStrategy();
                }
                return cache;
            }
        }
    }
}
