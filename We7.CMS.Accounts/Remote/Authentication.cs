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
        public static bool CreateAppToken(SSORequest ssoRequest)
        {
            string OriginalAuthenticator = ssoRequest.SiteID + ssoRequest.TimeStamp + ssoRequest.AppUrl;
            string AuthenticatorDigest = CryptoHelper.ComputeHashString(OriginalAuthenticator);
            string sToEncrypt = OriginalAuthenticator + AuthenticatorDigest;
            byte[] bToEncrypt = CryptoHelper.ConvertStringToByteArray(sToEncrypt);

            CryptoService cs = GetCryptoService();
            byte[] encrypted;
            if (cs.Encrypt(bToEncrypt, out encrypted))
            {
                ssoRequest.Authenticator = CryptoHelper.ToBase64String(encrypted);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetAppKey()
        {
            return "22362E7A9285DD53A0BBC2932F9733C505DC04EDBFE00D70"; //47char
        }

        public static string GetAppIV()
        {
            return "1E7FA9231E7FA923";  //16char
        }

        public static CryptoService GetCryptoService()
        {
            string key = GetAppKey();
            string IV = GetAppIV();

            CryptoService cs = new CryptoService(key, IV);
            return cs;
        }

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

        public static void Post(SSORequest ssoRequest, string url)
        {
            PostService ps = new PostService();
            ps.Url = url;
            ps.Add("Action", ssoRequest.Action);
            if (!string.IsNullOrEmpty(ssoRequest.SiteID))
                ps.Add("SiteID", ssoRequest.SiteID);
            if (!string.IsNullOrEmpty(ssoRequest.AccountID))
                ps.Add("AccountID", ssoRequest.AccountID);
            if (!string.IsNullOrEmpty(ssoRequest.UserName))
                ps.Add("UserName", ssoRequest.UserName);
            if (!string.IsNullOrEmpty(ssoRequest.Password))
                ps.Add("Password", ssoRequest.Password);
            ps.Add("TimeStamp", ssoRequest.TimeStamp);
            ps.Add("AppUrl", ssoRequest.AppUrl);
            ps.Add("Authenticator", ssoRequest.Authenticator);

            ps.Post();
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
