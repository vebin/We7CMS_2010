using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace We7.Framework.Util
{
    public class Utils
    {
        public static void Redirect(string url, bool endResponse = true)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Redirect(url, endResponse);
            }
        }
    }
}
