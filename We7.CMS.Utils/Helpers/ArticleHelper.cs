using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using Thinkment.Data;
using System.Web;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.ArticleHelper")]
    public class ArticleHelper : BaseHelper
    {
        public ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        public HttpContext context
        {
            get { return HttpContext.Current; }
        }

        public HelperFactory HelperFactory
        {
            get { return (HelperFactory)context.Application[HelperFactory.ApplicationID]; }
        }


        public List<Article> QueryArticlesByAll(ArticleQuery query)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);
            }
            catch (Exception ex)
            { }
        }

        Criteria CreateCriteriaByAll(ArticleQuery query)
        {
            string parameters;
            string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
            Criteria c = new Criteria(CriteriaType.None);
        }
    }
}