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

        string productVersion
        {
            get 
            {
                string fullVersion = GeneralConfigs.GetConfig().ProductVersion;
                return Regex.Replace(fullVersion, @"\.\d+\.\d+\s+[\u4e00-\u9fa5]+", "");
            }
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