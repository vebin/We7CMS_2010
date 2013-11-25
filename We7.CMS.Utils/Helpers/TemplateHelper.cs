using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using System.IO;
using System.Web;

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
