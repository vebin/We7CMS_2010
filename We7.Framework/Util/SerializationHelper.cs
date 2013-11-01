using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace We7.Framework.Util
{
    public class SerializationHelper
    {
        private SerializationHelper()
        { }

        public static object Load(string filename, Type type)
        {
            FileStream fs = null;
            object obj = null;

            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                obj = serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return obj;
        }

        public static bool Save(object obj, string filename)
        {
            FileStream fs = null;
            FileInfo fi = new FileInfo(filename);
            if (!fi.Directory.Exists)
                fi.Directory.Create();   
            bool successed = false;

            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                successed = true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            return successed;
        }
    }
}
