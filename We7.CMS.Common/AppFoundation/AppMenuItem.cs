using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.AppFoundation
{
    [Serializable]
    public class AppMenuItem
    {
        public AppMenuItem(string id, string name, string title, string url, bool main, string entityID)
        {
            ID = id;
            Name = name;
            Title = title;
            Url = url;
            Main = main;
            EntityID = entityID;
        }

        public string ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool Main { get; set; }

        public string EntityID { get; set; }
    }
}
