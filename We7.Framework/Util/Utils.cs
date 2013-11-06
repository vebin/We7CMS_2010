using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

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

        public static string GetRootUrl()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            Regex reg = new Regex(@"http\s*:\s*//[^/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = reg.Match(url);
            return match.Success ? match.Value : string.Empty;
        }

        public static void TrimsEndStringBuilder(StringBuilder sb, string endStr)
        {
            if (sb.Length > 0 && sb.ToString().EndsWith(endStr))
            {
                sb.ToString().Substring(0, sb.ToString().LastIndexOf(endStr));
            }
        }
    }
}
