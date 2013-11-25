using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using System.IO;
using System.Xml;
using We7.CMS.Common;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class NewSiteWizard : BasePage
    {
        protected override MasterPageMode MasterPageIs        {
            get             {
                if (Request["nomenu"] != null)
                    return MasterPageMode.NoMenu;
                else
                    return MasterPageMode.FullMenu;            }        }

        public string OriginPath
        {
            get { return ViewState["_OriginPath"] as string; }
            set { ViewState["_OriginPath"] = value; }
        }

        protected override void Initialize()
        {
            if (Request["nomenu"] != null)
            {

            }
            else
            { 
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!ValidateUpload())
            {
                ltlMsg.Text = "<br/><font color='red'>文件为空或文件格式不对</font>";
                return;
            }
            UploadFile();
        }

        protected void deleteGroupButton_Click(object sender, EventArgs e)
        {
            Save();

        }

        protected string CheckLength(string content, int len)
        {
            if (content.Length > len)
                return string.Format("{0}...", content.Substring(0, len));
            else
                return content;
        }

        protected string GetImageUrl(string filename)
        {
            string previewFileName = filename + ".jpg";
            string path = "/" + Path.Combine(Constants.TemplateGroupBasePath, previewFileName);
            if (!File.Exists(path))
                path = "images/template_default.jpg";
            string phyPath = path.Replace("\\", "/");

            return phyPath;
        }

        protected string GetTemplateGroupUrl(string filename, string str)
        {
            return GetTemplateGroupUrl(filename, "", str);
        }

        protected string GetTemplateGroupUrl(string filename, string groupname, string action)
        {
            switch (action)
            { 
                case "编辑":
                    string url = "";
                    string name = filename;
                    if (!name.ToLower().EndsWith(".xml"))
                        name += ".xml";
                    string file = Path.Combine(TemplateHelper.TemplateGroupPath, name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNode node = doc.SelectSingleNode("TemplateGroup");
                    XmlAttribute attr = node.Attributes["ver"];
                    url = string.Format("/admin/TemplateGroupDetail.aspx?file={0}", filename);
                    if (attr != null)
                    {
                        string version = attr.Value;
                        if (!string.IsNullOrEmpty(version))
                        {
                            if (version.StartsWith("V"))
                                version.Remove(0, 1);
                            if (string.CompareOrdinal(version, "2.1") >= 0)     //增加此判断的原因？
                                url = string.Format("/admin/TemplateGroupEdit.aspx?file={0}", filename);
                        }
                    }
                    return url;
                case "删除":
                    return string.Format("javascript:deleteGroup('{0}', '{1}')", groupname, filename);
                case "应用":
                    return string.Format("javascript:applyGroup('{0}', '{1}')", groupname, filename);
                case "打包":
                    return string.Format("TemplateGroupDownload.aspx?file={0}", filename);
                default:
                    return "";
            }
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }

        string GetFileFolder()
        {
            Article article = new Article();
            article.ID = We7Helper.CreateNewID();
            return article.AttachmentUrlPath.TrimEnd("/".ToCharArray()) + "/Thumbnail";
        }

        void Save()
        {
            if (CDHelper.Config.IsDemoSite)
                return;
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            gi.SiteTitle = ViewState["SiteTitle"].ToString();
            gi.Copyright = ViewState["Copyright"].ToString();
            gi.SiteFullName = ViewState["SiteFullName"].ToString();
            gi.IcpInfo = ViewState["IcpInfo"].ToString();
            gi.SiteLogo = ViewState["SiteLogo"].ToString();
            lblSiteName.Text = ViewState["SiteTitle"].ToString();
            GeneralConfigs.SaveConfig(gi);
        }

        bool ValidateUpload()
        {
            if (string.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim(',');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return We7.Framework.Util.Utils.InArray(ext.ToLower(), list);
        }

        void UploadFile()
        {
            string ext = Path.GetExtension(fuImage.FileName);
            string folder = GetFileFolder();
            string newFileName = CreateFileName();

            OriginPath = folder.TrimEnd('/')+"/"+newFileName+ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
                Directory.CreateDirectory(physicalpath);
            string physicalfilepath = Server.MapPath(OriginPath);
            fuImage.SaveAs(physicalfilepath);
            ImageValue.Text = OriginPath;
        }
    }
}