using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace We7.CMS
{
    public partial class TemplateHelper : BaseHelper
    {
        //public string GetHtmlTemplateByHandlers(string columnMode, string columnID, string searchWord, string seSearchWord)
        //{ 
            
        //}
       
        void ClearSkinInfoCache(string filename)
        {
            string key = "CD.SkinInfo." + filename;
            HttpContext.Current.Application.Remove(key);
        }

        public string TemplateGroupPath
        {
            get { return Path.Combine(Root, Constants.TemplateGroupBasePath); }
        }

        public void DeleteTemplateGroup(string groupname)
        {
            string target = Path.Combine(TemplateGroupPath, groupname);
            if (File.Exists(target))
                File.Delete(target);
            string targetImage = Path.Combine(TemplateGroupPath, groupname+".jpg");
            if (File.Exists(targetImage))
                File.Delete(targetImage);
            string targetFile = Regex.Split(target, ".xm", RegexOptions.IgnoreCase)[0];
            if (Directory.Exists(targetFile))
            {
                DirectoryInfo di = new DirectoryInfo(targetFile);
                We7Helper.DeleteFileTree(di);
            }
        }

        public SkinInfo GetTemplateGroup(string filename)
        {
            SkinInfo info = new SkinInfo();
            info.FromFile(TemplateGroupPath, filename);
            return info;
        }

        public string SaveSkinInfoAndPreviewFile(SkinInfo data, string foldername)
        {
            if (data.FileName == null)
            {
                data.Created = DateTime.Now;
                data.FileName = string.Format("{0}.xml", foldername);
            }
            data.ToFile(TemplateGroupPath, data.FileName);
            ClearSkinInfoCache(data.FileName);

            return data.FileName;
        }
    }
}
