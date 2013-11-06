using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework.Util;
using System.Web;

namespace We7.CMS.Accounts
{
    public class Authentication
    {
        public static string GetCurrentUrl(string toUrls, ref string leavesToUrls)
        {
            string url = string.Empty;
            if (!String.IsNullOrEmpty(toUrls) && !String.IsNullOrEmpty(toUrls.Trim()))
            {
                string[] urls = toUrls.Split(';');
                url = urls[0];
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < urls.Length; i++)
                {
                    sb.Append(urls[i] + ";");
                }
                Utils.TrimsEndStringBuilder(sb, ";");
                leavesToUrls = sb.ToString();
            }
            return url;
        }

        public static void PostChains(SSORequest ssoRequest)
        {
            string leavesToUrls = string.Empty;
            string url = GetCurrentUrl(ssoRequest.ToUrls, ref leavesToUrls);
            ssoRequest.ToUrls = leavesToUrls;

            if (!String.IsNullOrEmpty(url))
            {
                PostService ps = new PostService();
                ps.Url = url;
                ps.Add("Action", ssoRequest.Action);
                ps.Add("ToUrls", ssoRequest.ToUrls);
                if (!String.IsNullOrEmpty(ssoRequest.UserName))
                    ps.Add("UserName", ssoRequest.UserName);
                if (!String.IsNullOrEmpty(ssoRequest.Password))
                    ps.Add("Password", ssoRequest.Password);
                ps.Add("AppUrl", ssoRequest.AppUrl);
                ps.Post();
            }
            else
            {
                HttpContext.Current.Response.Redirect(ssoRequest.AppUrl);
            }
        }

    }
}
