using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Config;
using System.Web.UI.WebControls;
using System.IO;
using We7.Framework.Config;
using System.Security.Cryptography;

namespace We7.CMS.Install
{
    public class Step3 : SetupPage
    {
        protected TextBox DatabaseTextBox;
        protected TextBox ServerTextBox;
        protected TextBox UserTextBox;
        protected TextBox PasswordTextBox;
        protected TextBox DBFileNameTextBox;
        protected CheckBox CreateNewDBCheckBox;
        protected Literal msg;
        protected TextBox WebsiteNameTextBox;
        protected TextBox AdminNameTextBox;
        protected DropDownList DBTypeDropDownList;
        protected TextBox AdminPasswordTextBox;
        protected TextBox txtMsg;
        protected Panel ConfigMsgPanel;
        protected Button ResetDBInfo;
        

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (BaseConfigs.ConfigFileExist() && !LockFileExist())
            {
                Response.Redirect("upgrade.aspx", true);
                Init();
            }
        }

        private string Encrypt(string password)
        {
            byte[] clearBytes = new UnicodeEncoding().GetBytes(password.ToLower());
            byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }

        private void ErrProcess(BaseConfigInfo bci)
        {
            string MyDBConfig = "<?xml version=\"1.0\"?>\r\n";
            MyDBConfig += "<BaseConfigInfo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n";
            MyDBConfig += "<DBConnectionString>"+bci.DBConnectionString+"</DBConnectionString>\r\n";
            MyDBConfig += "<DBDriver>"+bci.DBDriver+"</DBDriver>";
            MyDBConfig += "<DBType>"+bci.DBType+"</DBType>";
            MyDBConfig += "</BaseConfigInfo>";
            txtMsg.Text = MyDBConfig;
            ConfigMsgPanel.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
                if (bci != null)
                {
                    DatabaseInfo dbi = new DatabaseInfo();
                    dbi = Installer.GetDatabaseInfo(bci);
                    ServerTextBox.Text = dbi.Server;
                    DatabaseTextBox.Text = dbi.Database;
                    UserTextBox.Text = dbi.User;
                    PasswordTextBox.Text = dbi.Password;
                    DBFileNameTextBox.Text = dbi.DBFile;
                    if (DBFileNameTextBox.Text.IndexOf("\\") >= 0)
                    {
                        DBFileNameTextBox.Text = DBFileNameTextBox.Text.Substring(DBFileNameTextBox.Text.LastIndexOf("\\") + 1);
                    }
                    SelectDB = bci.DBType;
                    CreateNewDBCheckBox.Checked = false;
                    if (!CheckWebConfig())
                    {
                        msg.Visible = true;
                    }
                    SiteConfigInfo sci = SiteConfigs.GetConfig();
                    if (sci != null)
                    {
                        WebsiteNameTextBox.Text = sci.SiteName;
                        AdminNameTextBox.Text = sci.AdministratorName;
                    }
                }
            }
        }

        protected void ResetDBInfo_Click(object sender, EventArgs e)
        {
            if (AdminPasswordTextBox.Text.Length < 6)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('系统管理员密码不能少于6位!');</script>");
                return;
            }
            if (DatabaseTextBox.Text.Length == 0 && DBTypeDropDownList.SelectedValue == "SqlServer")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>数据库名称不能为空!</script>");
                return;
            }
            if (DBTypeDropDownList.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请选择数据库类型!');</script>");
                return;
            }

            try
            {
                SiteConfigInfo __configinfo;
                try
                {
                    __configinfo = SiteConfigs.GetConfig();
                }
                catch
                {
                    __configinfo = new SiteConfigInfo();
                }
                __configinfo.AdministratorKey = Encrypt(AdminPasswordTextBox.Text);
                __configinfo.IsPasswordHashed = true;
                __configinfo.SiteName = WebsiteNameTextBox.Text;
                __configinfo.AdministratorName = AdminNameTextBox.Text;
                SiteConfigs.Serialize(__configinfo, Server.MapPath("~/Config/site.config"));
                Session["SystemAdminName"] = AdminNameTextBox.Text;
                Session["SystemAdminPwd"] = AdminPasswordTextBox.Text;
            }
            catch
            { ;}

            string setupDBType = SelectDB = DBTypeDropDownList.SelectedValue;
            DatabaseInfo dbi = new DatabaseInfo();
            dbi.Server = ServerTextBox.Text;
            dbi.Database = DatabaseTextBox.Text;
            dbi.User = UserTextBox.Text;
            dbi.Password = PasswordTextBox.Text;
            dbi.DBFile = DBFileNameTextBox.Text;

            BaseConfigInfo bci = Installer.GenerateConnectionString(setupDBType, dbi);
            if (!SaveDBConfig(bci))
            {
                ResetDBInfo.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>if(confirm('无法把设置写入\"db.config\"文件，系统将把文件内容显示出来，您可以将内容保存为\"db.config\"，然后通过FTP软件上传到网站根目录下.. \\r\\n*注意：db.config位于网站Config目录下。\\r\\n\\r\\n如要继续运行安装，请按\"确定\"按钮.')){window.location.href='step4.aspx?isforceload=1';}else{window.location.href='step3.aspx';}</script>");
                return;
            }

            if (bci.DBType == "Oracle" || bci.DBType == "MySql")
            {
                CreateNewDBCheckBox.Checked = false;
            }

            if (CreateNewDBCheckBox.Checked)
            {
                Exception ex = null;
                int ret = Installer.CreateDatabase(bci, out ex);
            }
        }

        public bool CheckWebConfig()
        {
            string webconfigpath = Path.Combine(Request.PhysicalApplicationPath, "Web.config");
            if (!File.Exists(webconfigpath) && !File.Exists(Server.MapPath("../Web.config")))
            {
                return false;
            }
            return true;
        }

        private bool SaveDBConfig(BaseConfigInfo bci)
        {
            try
            {
                string file = Server.MapPath("~/Config/db.config");
                BaseConfigs.SaveConfigTo(bci, file);
                BaseConfigs.ResetConfig();
                SetupPage.CreateLockFile();
                return true;
            }
            catch
            {
                ErrProcess(bci);
            }
            return false;
        }
    }
}
