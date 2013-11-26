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
                TitleLabel.Text = "新建站点向导";
                SummaryLabel.Text = "分三步创建新站点";
            }
            else
            {
                TitleLabel.Text = "站点设置向导";
                SummaryLabel.Text = "分三步设置站点";
            }
            CheckDisplay();
            BindSiteConfig();
            BindTemplate();
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

        protected void applyGroupButton_Click(object sender, EventArgs e)
        {
            Save();
            ApplyDefaultTemplate(currentGroup.Text);
            Response.Write("<script>document.location.reload();</script>");
            Response.Redirect(Request.RawUrl);
        }

        protected void deleteGroupButton_Click(object sender, EventArgs e)
        {
            Save();
            TemplateHelper.DeleteTemplateGroup(currentGroup.Text);
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            if (gi.DefaultTemplateGroupFileName.ToLower() == currentGroup.Text.ToLower())
            {
                gi.DefaultTemplateGroupFileName = "";
                GeneralConfigs.SaveConfig(gi);
                GeneralConfigs.ResetConfig();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void PreviousPanel(object sender, EventArgs e)
        {
            if (pnlSiteTemplate.Visible)
            {
                Session["VisibleIndex"] = 1;
                pnlSiteTemplate.Visible = false;
                pnlSiteConfig.Visible = true;
            }
        }

        protected void btnNextPanel(object sender, EventArgs e)
        {
            if (pnlSiteConfig.Visible)
            {
                Session["VisibleIndex"] = 2;
                pnlSiteTemplate.Visible = true;
                pnlSiteConfig.Visible = false;
                PanelSuccess.Visible = false;
                BindViewState();
            }
            else if (pnlSiteTemplate.Visible)
            {
                Save();
                Session["VisibleIndex"] = 3;
                pnlSiteConfig.Visible = false;
                pnlSiteTemplate.Visible = false;
                PanelSuccess.Visible = true;
                btnNext.Visible = false;
                btnPrevious.Visible = false;
            }
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

        void ApplyDefaultTemplate(string filename)
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            gi.DefaultTemplateGroupFileName = filename;
            GeneralConfigs.SaveConfig(gi);
            GeneralConfigs.ResetConfig();
        }

        void BindSiteConfig()
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            txtSiteName.Text = gi.SiteTitle;
            txtCopyright.Text = gi.Copyright;
            txtSiteFullName.Text = gi.SiteFullName;
            txtIcpInfo.Text = gi.IcpInfo;
            ImageValue.Text = gi.SiteLogo;
            lblSiteName.Text = gi.SiteTitle;
            BindViewState();
        }

        void BindTemplate()
        {
            string msg = @"您尚未指定当前模板组，您可以：<br/>" +
                "(1) 在下面可选模板组中，选择一个，点击“应用”；<br/>" +
                "(2) 创建一个新的模板组（上面工具条中点击“创建模板组”）；<br/>" +
                "(3) 上传一个模板组包（工具条中点击“上传模板”）。";
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            if (!string.IsNullOrEmpty(gi.DefaultTemplateGroupFileName))
            {
                List<SkinInfo> skins = new List<SkinInfo>();
                try
                {
                    skins.Add(TemplateHelper.GetTemplateGroup(gi.DefaultTemplateGroupFileName));
                    if (skins.Count < 1)
                        UploadHyperLink.Visible = true;
                    UseTemplateGroupsDataList.DataSource = skins;
                    UseTemplateGroupsDataList.DataBind();
                }
                catch
                {
                    UploadHyperLink.Visible = true;
                    UseTemplateGroupsLabel.Text = msg;
                }
            }
            else
            {
                UploadHyperLink.Visible = true;
                UseTemplateGroupsLabel.Text = msg;
            }
            UseTemplateGroupsDataList.DataSource = TemplateHelper.GetTemplateGroup(null);
            UseTemplateGroupsDataList.DataBind();
        }

        void BindViewState()
        {
            ViewState["SiteTitle"] = txtSiteName.Text;
            ViewState["Copyright"] = txtCopyright.Text;
            ViewState["SiteFullName"] = txtSiteFullName.Text;
            ViewState["IcpInfo"] = txtIcpInfo.Text;
            ViewState["SiteLogo"] = ImageValue.Text;
        }

        void CheckDisplay()
        {
            btnNext.Visible = true;
            btnPrevious.Visible = true;
            if (Session["VisibleIndex"] == null)
            {
                pnlSiteConfig.Visible = true;
                pnlSiteTemplate.Visible = false;
                PanelSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "1")
            {
                pnlSiteConfig.Visible = true;
                pnlSiteTemplate.Visible = false;
                PanelSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "2")
            {
                pnlSiteConfig.Visible = false;
                pnlSiteTemplate.Visible = true;
                PanelSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "3")
            {
                Session["VisibleIndex"] = null;
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
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
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