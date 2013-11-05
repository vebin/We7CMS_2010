using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace We7.Framework.Util
{
    public class FileHelper
    {
        public static string ReadFile(string filePath, Encoding encoding)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    using (StreamReader sr = new StreamReader(filePath, encoding))
                    {
                        string temp = string.Empty;

                        while ((temp = sr.ReadLine()) != null)
                        {
                            sb.Append(temp);
                        }
                    }

                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("文件不存在！");
            }
        }
    }
}
