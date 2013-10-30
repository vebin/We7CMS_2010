using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace We7.Framework
{
    public class LogHelper
    {
        public static string sql_update = @"Install/sql_update_log"+DateTime.Today.ToString("yyyyMM")+".txt";

        public static void WriteFileLog(string filename, string strTitle, string strContent)
        {
            string strFile = "";
            string strDir = "";
            if (!Path.IsPathRooted(filename))
            {
                strDir = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Logs\";
            }
            string[] parts = strDir.Replace("/", "\\").Split('\\');
            if (parts.Length > 1)
            {
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    strDir += parts[i] + @"\";
                    if (!Directory.Exists(strDir))
                        Directory.CreateDirectory(strDir);
                }
                strFile = strDir + parts[parts.Length - 1];
            }
            else
                strFile = strDir + filename;

            DateTime now = DateTime.Now;
            StreamWriter sw = File.Exists(strFile) ? File.AppendText(strFile) : File.CreateText(strFile);
            sw.WriteLine("<---------------------{0}--------------------->", now.ToLongDateString()+" "+now.ToLongTimeString());
            sw.WriteLine("Title:");
            sw.WriteLine("\t"+strTitle);
            sw.WriteLine("Content:");
            sw.WriteLine("\t"+strContent);
            sw.WriteLine("");
            sw.Close();
        }
    }
}
