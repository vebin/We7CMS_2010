using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class HelperAttribute : Attribute
    {
        string helpername;
        string description;

        public HelperAttribute(string name)
        {
            helpername = name;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Name
        {
            get { return helpername; }
            set { helpername = value; }
        }
    }
}
