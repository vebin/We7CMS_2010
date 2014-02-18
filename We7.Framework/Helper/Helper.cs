using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using We7.Framework.Config;
using System.Web;
using System.Data.SQLite;
using System.Data;

namespace We7
{
    public static class We7Helper
    {
        public static void AssertNotNull(object obj, string errMsg)
        {
            string s = string.Format("{0} in null", errMsg);
            Assert(obj != null && obj != DBNull.Value, s);
        }

        public static void Assert(bool NotNull, string msg)
        {
            if (!NotNull)
            {
                throw new Exception(msg);
            }
        }

        public static string EmptyGUID
        {
            get { return "{00000000-0000-0000-0000-000000000000}"; }
        }

        public static string EmptyWapGUID
        {
            get
            {
                return "{00000000-1111-0000-0000-000000000000}";
            }
        }

        public static string ConvertTextToHtml(string text)
        {
            text.Replace("<", "&lt;");
            text.Replace(">", "&gt;");
            text.Replace("'", "\"");
            text.Replace(" ", "&nbsp;");
            text.Replace("\r\n", "<br/>");
            text.Replace("\r", "<br/>");
            text.Replace("\n", "<br/>");
            text.Replace("\"", "&quot;");

            return text;
        }

        public static string CreateNewID()
        { 
            return "{"+Guid.NewGuid().ToString()+"}";
        }

        public static void DeleteFileTree(DirectoryInfo di)
        {
            foreach (DirectoryInfo info in di.GetDirectories())
            {
                DeleteFileTree(info);
            }

            foreach (FileInfo info in di.GetFiles())
            {
                File.SetAttributes(info.FullName, FileAttributes.Normal);
                info.Delete();
            }
            Directory.Delete(di.FullName);
        }

        public static string GetCityNameByIP(string ip)
        {
            string city = "";
            string strCon = GeneralConfigs.GetConfig().IPDBConnection;//连接字符串
            HttpContext Context = HttpContext.Current;
            string dataPath = Context.Server.MapPath("~/App_Data/DB");
            strCon = strCon.Replace("{$Current}", dataPath);
            SQLiteConnection myConnection = new SQLiteConnection(strCon);
            string strSel = "SELECT * FROM [IPToCity] WHERE [IP_Start]<={0} AND [IP_End]>={0}";
            strSel = string.Format(strSel, IPToINT(ip).ToString());
            SQLiteCommand cmd = new SQLiteCommand(strSel, myConnection);
            try
            {
                cmd.Connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    city = myReader["IP_Province"].ToString() + " " + myReader["IP_City"].ToString();
                }
            }
            catch
            { }

            if (myConnection != null && myConnection.State == ConnectionState.Open)
            {
                myConnection.Close();
            }
            return city;
        }

        public static string GetDomainFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            if (url.ToLower().StartsWith("http://"))
                url = url.Remove(0, 7);
            string[] parts = url.Split('/');
            string domain = string.Empty;
            List<string> dotcoms = new List<string>() { ".com", ".org", ".net", ".gov"};
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "" && parts[i].IndexOf(".") > 0)
                {
                    domain = parts[0];
                    break;
                }
            }
            if (domain.Length > 0)
            {
                string[] ds = domain.Split('.');
                if (ds.Length == 3 && !dotcoms.Contains(ds[1]))
                    domain = ds[1] + "." + ds[2];
                else if (ds.Length == 4 && dotcoms.Contains(ds[2]))
                    domain = ds[1] + "." + ds[2] + "." + ds[3];
                else if (ds.Length > 4)
                    domain = ds[ds.Length - 3] + "." + ds[ds.Length - 2] + "." + ds[ds.Length - 1];
            }
            return domain;
        }

        public static string GUIDToFormatString(string guid)
        {
            if (guid == null || guid == "")
                return "";
            string ret = guid.Replace("{", "").Replace("}", "");
            return ret.Replace("-", "_");
        }

        public static long IPToINT(string ip)
        {
            string[] parts = ip.Split('.');
            long[] ids = new long[] { 0,0,0,0 };
            if (parts.Length == 4)
            {
                ids[0] = long.Parse(parts[0]);
                ids[1] = long.Parse(parts[1]);
                ids[2] = long.Parse(parts[2]);
                ids[3] = long.Parse(parts[3]);

                long id = ids[0] * 256 * 256 * 256 + ids[1] * 256 * 256 + ids[2] * 256 + ids[3];
                return id;
            }
            else
                return -1;
        }

        public static bool IsEmptyID(string id)
        {
            return id == null || id == string.Empty || id == EmptyGUID || id == EmptyWapGUID;
        }
    }
}
