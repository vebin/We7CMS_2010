using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thinkment.Data;
using System.Web.Caching;
using System.Web;

namespace We7.Framework
{
    /// <summary>
    /// 缓存时间
    /// </summary>
    public enum CacheTime
    { 
        Short = 10,
        Medium = 20,
        Long = 30
    }

    [Serializable]
    public abstract class BaseHelper : IHelper
    {
        ObjectAssistant assistant;
        public ObjectAssistant Assistant
        {
            get 
            {
                if (assistant == null)
                {
                    assistant = HelperFactory.Instance.Assistant;
                }
                return assistant;
            }
            set { assistant = value; }
        }

        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        string root;
        public string Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }

        public void CacherCache(string key, HttpContext context, object obj, CacheTime ct)
        {
            if (obj != null)
            {
                context.Cache.Insert(key, obj, null, DateTime.Now.AddSeconds((int)ct), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        protected List<Order> CreateOrdersByAll(string orderString)
        { 
            List<Order> orders = new List<Order>();
            if (!string.IsNullOrEmpty(orderString))
            {
                orderString = orderString.Replace(" ", "");
                string[] keyValues = orderString.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in keyValues)
                {
                    string[] tmps = item.Split(new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
                    string key = "Updated";
                    string value = "Asc";
                    if (tmps.Length > 0)
                    { 
                        key = tmps[0];
                        if (tmps.Length > 1)
                        { 
                            value = tmps[1];
                        }
                    }
                    Order o = new Order(key);
                    o.Mode = (OrderMode)System.Enum.Parse(typeof(OrderMode), value, true);
                    orders.Add(o);
                }
            }
            if (orders.Count < 1)
            {
                orders = null;
            }
            return orders;
        }
    }
}
