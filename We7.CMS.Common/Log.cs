using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class Log
    {
        string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        string userID;
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        DateTime created = DateTime.Now;
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        string page;
        public string Page
        {
            get { return page; }
            set { page = value; }
        }

        string remark;
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        DateTime updated = DateTime.Now;
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
    }
}
