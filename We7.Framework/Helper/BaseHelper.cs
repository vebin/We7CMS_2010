using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thinkment.Data;

namespace We7.Framework
{
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
    }
}
