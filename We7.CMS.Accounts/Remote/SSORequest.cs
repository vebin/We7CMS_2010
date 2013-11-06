using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace We7.CMS.Accounts
{
    [Serializable]
    public class SSORequest : MarshalByRefObject
    {
        public SSORequest()
        {
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            AppUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + HttpContext.Current.Request.RawUrl;
        }

        public string Action { get; set; }

        public string AppUrl { get; set; }

        public string Password { get; set; }

        public string TimeStamp { get; set; }

        public string ToUrls { get; set; }

        public string UserName { get; set; }
    }
}
