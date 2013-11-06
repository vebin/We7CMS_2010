using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Config;
using We7.CMS.Common.Enum;
using System.IO;
using We7.Framework.Util;
using System.Text;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin
{
    public partial class Signin : BasePage
    {
        public string ProductBrand
        {
            get 
            {
                GeneralConfigInfo gi = GeneralConfigs.GetConfig();
                if (gi != null)
                {
                    return gi.ProductName;
                }
                else
                    return "We7";
            }
        }

        protected override bool NeedAnAccount
        {
            get
            {
                return false;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ReturnURL
        {
            get
            {
                if (Request["ReturnURL"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Server.UrlDecode(Request["ReturnURL"].ToString());
                }
            }
        }

        public bool CheckLocalAdministrator(string loginname, string password, string siteid)
        {
            if (SiteConfigs.GetConfig().SiteGroupEnabled == true)
            {
                return true;
            }
            else
            {
                if (String.Compare(loginname, SiteConfigs.GetConfig().AdministratorName, true) == 0)
                {
                    if (SiteConfigs.GetConfig().IsPasswordHashed)
                    {
                        password = Security.Encrypt(password);
                    }
                    string hashpwd = SiteConfigs.GetConfig().AdministratorKey;
                    return String.Compare(password, hashpwd, true) == 0;
                }
                else
                    return false;
            }
        }

        private bool checkLicense()
        {
            bool result = true;
            try
            {
                string filePath = Server.MapPath("~/admin/exp.txt");
                if (File.Exists(filePath))
                {
                    string content = FileHelper.ReadFile(filePath, Encoding.Default);
                    DateTime expDate;
                    if (DateTime.TryParse(content, out expDate)
                    {
                        if (DateTime.Now >= expDate)
                        {
                            result = false;
                        }
                    }
                }
            }
            catch
            {}

            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!checkLicense())
            {
                ShowMessage("您的系统授权已经过期，请及时联系客服！");
                return;
            }
            if (!Page.IsPostBack)
            {
                GeneralConfigInfo gi = GeneralConfigs.GetConfig();
                if (gi.IsOEM)
                    CopyrightLiteral.Text = gi.Copyright;
                else
                    CopyrightLiteral.Text = gi.CopyrightOfWe7;
                SiteConfigInfo si = SiteConfigs.GetConfig();
                if (si == null)
                {
                    Response.Write("对不起，您的系统已经升级，但配置文件尚未升级，您需要对配置数据进行升级。现在升级吗？<a href='../install/upgradeconfig.aspx'><u>现在升级</u></a>");
                    Response.End();
                }
                else
                {
#if DEBUG
                    LoginNameTextBox.Text = si.AdministratorName;    
#endif
                    GenerateRandomCode();

                }
            }
        }

        private void GenerateRandomCode()
        {
            if (CDHelper.Config.EnableLoginAuhenCode == "true")
            {
                tbAuthCode2.Visible = true;
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
            }
        }

        private void ShowMessage(string msg)
        {
            MessageLabel.Text = msg;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoginAction(LoginNameTextBox.Text.Trim(), PasswordTextBox.Text);
        }

        void LoginAction(string loginName, string password)
        {
            if (!checkLicense())
            {
                ShowMessage("您的系统授权已经过期，请及时联系客服！");
                return;
            }
            if (String.IsNullOrEmpty(loginName) || String.IsNullOrEmpty(loginName.Trim()))
            {
                ShowMessage("错误：用户名不能为空！");
                return;
            }
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
            {
                ShowMessage("错误：密码不能为空！");
                return;
            }
            if (GeneralConfigs.GetConfig().EnableLoginAuhenCode == "true" && CodeNumberTextBox.Text != Request.Cookies["AreYouHuman"].Value)
            {
                ShowMessage("错误：您输入的验证码不正确，请重新输入！");
                CodeNumberTextBox.Text = "";
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
                return;
            }
            bool loginSuccess = false;

            if (CheckLocalAdministrator(loginName, password, SiteConfigs.GetConfig().SiteID))
            {
                Security.SetAccountID(We7Helper.EmptyGUID);
                loginSuccess = true;
                SSOLogin(loginName, password);
            }
        }

        private void SSOLogin(string loginName, string password)
        {
            if (!String.IsNullOrEmpty(GeneralConfigs.GetConfig().SSOSiteUrl))
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.ToUrls = GeneralConfigs.GetConfig().SSOSiteUrl;
                ssoRequest.AppUrl = string.Format("{0}/{1}", We7.Framework.Util.Utils.GetRootUrl(), String.IsNullOrEmpty(ReturnURL) ? "Admin/theme/main.aspx" : ReturnURL.TrimStart('/'));
                ssoRequest.Action = "signin";
                ssoRequest.UserName = loginName;
                ssoRequest.Password = password;
                Authentication.PostChains(ssoRequest);
            }
        }
    }
}