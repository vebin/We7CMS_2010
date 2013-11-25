using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;

namespace We7.Framework.Util
{
    public class Utils
    {
        public static object CreateInstance(string fullname)
        {
            if (!string.IsNullOrEmpty(fullname))
            {
                string[] s = fullname.Split(',');
                if (s.Length == 2)
                {
                    Assembly ass = Assembly.Load(s[1]);
                    if (ass != null)
                    {
                        return ass.CreateInstance(s[0]);
                    }
                }
            }
            throw new Exception("CreateInstance<T>::程序集不存在!");
        }

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

        public static bool InArray(string search, string[] arr)
        {
            return InArray(search, arr, false);
        }

        public static bool InArray(string search, string[] arr, bool caseInsensitive)
        {
            return GetArrayID(search, arr, caseInsensitive) >= 0;
        }

        public static int GetArrayID(string search, string[] arr, bool caseInsensitive)
        {
            if (caseInsensitive)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (search.ToLower() == arr[i].ToLower())
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (search == arr[i])
                        return i;
                }
            }

            return -1;
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
