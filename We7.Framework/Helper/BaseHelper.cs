using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thinkment.Data;

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
    }
}
