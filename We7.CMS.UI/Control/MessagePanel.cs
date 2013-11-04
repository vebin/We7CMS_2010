using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace We7.CMS.Controls
{
    [ToolboxData("<{0}:MessagePanel runat=\"server\"></{0}:MessagePanel>")]
    public class MessagePanel : Panel, INamingContainer
    {
        private const string VSKEY_MESSAGE = "Message";
        private const string VSKEY_ERROR = "Error";

        bool _showMessagePanel;
        bool _showErrorPanel;

        string _messageCssClass;
        string _errorCssClass;
        string _messageIconUrl;
        string _errorIconUrl;

        #region Accessors
        public MessagePanel()
        {
            ViewState[VSKEY_ERROR] = string.Empty;
            ViewState[VSKEY_ERROR] = string.Empty;
        }

        public string Message
        {
            get { return (string)ViewState[VSKEY_MESSAGE]; }
            set { ViewState[VSKEY_ERROR] = value; }
        }

        public string Error
        {
            get { return (string)ViewState[VSKEY_MESSAGE]; }
            set { ViewState[VSKEY_ERROR] = value; }
        }

        public bool ShowMessagePanel
        {
            get { return _showMessagePanel; }
            set { _showMessagePanel = value; }
        }

        public bool ShowErrorPanel
        {
            get { return _showErrorPanel; }
            set { _showErrorPanel = value; }
        }

        public string MessageCssClass
        {
            get
            {
                if (_messageCssClass == null)
                {
                    _messageCssClass = "MessagePanel";
                }
                return _messageCssClass;
            }
            set { _messageCssClass = value; }
        }

        public string ErrorCssClass
        {
            get
            {
                if (_errorCssClass == null)
                {
                    _errorCssClass = "ErrorPanel";
                }
                return _errorCssClass;
            }
            set { _errorCssClass = value; }
        }

        public string MessageIconUrl
        {
            get
            {
                if (_messageIconUrl == null)
                    _messageIconUrl = "/admin/images/ico_info.gif";
                return _messageIconUrl;
            }
            set { _messageIconUrl = value; }
        }

        public string ErrorIconUrl
        {
            get
            {
                if (_errorIconUrl == null)
                    _errorIconUrl = "/admin/images/ico_critical.gif";
                return _errorIconUrl;
            }
        }    
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (null != Page)
                Page.VerifyRenderingInServerForm(this);
            if (this.ShowErrorPanel)
            {
                Panel errorPanel = BuildPanel(this.Error, this.ErrorCssClass, this.ErrorIconUrl);
                this.Controls.Add(errorPanel);
            }
            if (this.ShowMessagePanel)
            {
                Panel messagePanel = BuildPanel(this.Message, this.MessageCssClass, this.MessageIconUrl);
                this.Controls.Add(messagePanel);
            }
            base.Render(writer);
        }

        protected virtual Panel BuildPanel(string messageText, string cssClass, string imageUrl)
        {
            Panel result = new Panel();

            if (imageUrl != null && cssClass.Length > 0)
                result.CssClass = cssClass;
            Table tb = new Table();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            if (imageUrl != null && imageUrl.Length > 0)
            {
                Image image = new Image();
                image.Attributes["src"] = imageUrl;
                tc.Controls.Add(image);
                tc.Attributes["width"] = "15";
                tr.Controls.Add(tc);
                tc = new TableCell();
            }
            tc.Controls.Add(new LiteralControl(messageText));
            tr.Controls.Add(tc);
            tb.Controls.Add(tr);
            Panel message = new Panel();
            message.Controls.Add(tb);
            result.Controls.Add(message);

            return result;
        }

        public void ShowMessage(string message)
        {
            ShowMessage(message, true);
        }

        public void ShowMessage(string message, bool clearExistingMsgs)
        {
            if (clearExistingMsgs)
                this.Message = message;
            else
                this.Message += ""+message;
            this.ShowMessagePanel = true;
            this.Visible = true;
        }

        public void ShowError(string error)
        {
            ShowError(error, true);
        }

        public void ShowError(string error, bool clearExistingErrs)
        {
            if (clearExistingErrs)
                this.Error = error;
            else
                this.Error += ""+error;
            this.ShowErrorPanel = true;
            this.Visible = true;
        }

        public void Clear()
        {
            this.Visible = false;
        }
    }
}
