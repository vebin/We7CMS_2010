using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.tools.widget
{
    public partial class wid_myprocess : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingData();
            }
        }

        public void BindingData()
        {
            ArticleQuery query = new ArticleQuery();
            query.State = ArticleStates.Checking;
            List<Article> GetAllArticles = null;
            //List<Article> GetAllArticles = ArticleHelper.QueryArticlesByAll(query);
            if (GetAllArticles == null)
                return;
        }
    }
}