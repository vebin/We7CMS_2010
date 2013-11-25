using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static string GUIDToFormatString(string guid)
        {
            if (guid == null || guid == "")
                return "";
            string ret = guid.Replace("{", "").Replace("}", "");
            return ret.Replace("-", "_");
        }
    }
}
