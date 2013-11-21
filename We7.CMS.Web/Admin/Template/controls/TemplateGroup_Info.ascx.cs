using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Utils;
using We7.Framework.Config;
using System.Text.RegularExpressions;
using We7.CMS.Common;
using System.IO;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroup_Info : BaseUserControl
    {
        protected SkinInfo Data
        {
            get { return ViewState["$VS_SKIN_DATA"] as SkinInfo; }
            set { ViewState["$VS_SKIN_DATA"] = value; }
        }

        string FileName
        {
            get 
            {
                if (Request["file"] != null && Request["file"].Trim().ToLower() == "default")
                    return CDHelper.Config.DefaultTemplateGroupFileName;
                else if (Request["file"] != null && Request["file"].Trim().ToLower() == "mobile")
                    return CDHelper.Config.MobileTemplateGroupFileName;
                else
                    return Request["file"];
            }
        }

        public string productVersion
        {
            get 
            {
                string fullVersion = GeneralConfigs.GetConfig().ProductVersion;
                return Regex.Replace(fullVersion, @"\.\d+\.\d+\s+[\u4e00-\u9fa5]+", "");
            }
        }

        
        bool CreateFolder(string foldername)
        {
            string folderpath = Server.MapPath("/"+string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, foldername));
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
                return true;
            }
            return false;    
        }

        bool ExistFolder(string foldername)
        {
            string folderPath = Server.MapPath("/"+string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, foldername));
            return Directory.Exists(folderPath);   
        }

        void GetSkinInfo()
        {
            if (FileName != null)
            { 
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetSkinInfo();
            ShowSkinInfo();

            string folderName = NameTextBox.Text.ToLower().Trim();
            if (!string.IsNullOrEmpty(folderName) && ExistFolder(folderName))
            {
                NameTextBox.Enabled = false;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage)
                return;
            if (Data == null)
                Data = new SkinInfo();
            Data.Name = NameTextBox.Text;
            Data.Description = DescriptionTextBox.Text;
            Data.Ver = productVersion;

            string foldername = NameTextBox.Text.Trim();
            string filename = "";
            if (CreateFolder(foldername))
            {
                filename = TemplateHelper.SaveSkinInfoAndPreviewFile(Data, foldername);

                string content = string.Format("编辑模板组“{0}”", Data.Name);
                string title = "编辑模板组";
                if (FileName == null || FileName == string.Empty)
                {
                    content = string.Format("新建模板组{0}", Data.Name);
                    title = "新建模板组";
                    GeneralConfigInfo config = GeneralConfigs.GetConfig();
                    if (string.IsNullOrEmpty(config.DefaultTemplateGroupFileName))
                    {
                        config.DefaultTemplateGroupFileName = foldername + ".xml";
                        GeneralConfigs.SaveConfig(config);
                        GeneralConfigs.ResetConfig();
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "parent.location='" + string.Format("{0}?file={1}.xml", "/admin/Template/TemplateGroupEdit.aspx", foldername) + "'", true);
                    }
                }
                AddLog(title, content);
            }
        }

        void ShowSkinInfo()
        {
            if (Data != null)
            {
                NameTextBox.Text = Data.Name;
                DescriptionTextBox.Text = Data.Description;
                CreatedLabel.Text = Data.Created.ToString();
                FileTextBox.Text = Data.FileName;
            }
        }
    }
}