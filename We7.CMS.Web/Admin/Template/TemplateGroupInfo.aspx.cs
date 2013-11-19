using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using System.IO;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupInfo : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }

        string FileName { get { return Request["file"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Control ct = LoadControl("../Template/controls/TemplateGroup_Info.ascx");
            ContentHolder.Controls.Add(ct);
            if (!string.IsNullOrEmpty(FileName))
                NameLabel.Text = string.Format("编辑模板组{0}基本信息", Path.GetFileNameWithoutExtension(FileName));
            else
                NameLabel.Text = "新建模板组";
        }
    }
}