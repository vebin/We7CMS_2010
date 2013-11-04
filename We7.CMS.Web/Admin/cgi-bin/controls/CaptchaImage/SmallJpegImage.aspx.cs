using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using System.Drawing.Imaging;

namespace We7.CMS.Web.Admin
{
    public partial class SmallJpegImage : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            CaptchaImage ci = new CaptchaImage(Request.Cookies["AreYouHuman"].Value, 120, 30, "黑体");
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
            ci.Dispose();
        }
    }
}