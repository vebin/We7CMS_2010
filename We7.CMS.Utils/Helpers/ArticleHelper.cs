using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using Thinkment.Data;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using We7.CMS.Accounts;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.ArticleHelper")]
    public class ArticleHelper : BaseHelper
    {
        public IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

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

        public static void AppendModelCondition(Criteria c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("参数不能为代");
            }
            c.Criterias.Add(CreateModelCondition());
        }

        public static Criteria CreateModelCondition()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            c.AddOr(CriteriaType.Equals, "ModelName", Constants.ArticleModelName);
            c.AddOr(CriteriaType.Equals, "ModelName", string.Empty);
            c.AddOr(CriteriaType.IsNull, "ModelName", null);
            return c;
        }

        public List<Article> QueryArticlesByAll(ArticleQuery query)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);
                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(c, o);
            }
            catch (Exception ex)
            { }
        }

        Criteria CreateCriteriaByAll(ArticleQuery query)
        {
            string parameters;
            string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
            Criteria c = new Criteria(CriteriaType.None);
            if (string.IsNullOrEmpty(modelname) && !string.IsNullOrEmpty(query.EnumState))
            {
                Criteria csubC = new Criteria();
                csubC.Name = "EnumState";
                csubC.Value = query.EnumState;
                if (query.ExcludeThisChannel)
                    csubC.Type = CriteriaType.NotEquals;
                else
                    csubC.Type = CriteriaType.Equals;
                csubC.Adorn = Adorns.SubString;
                csubC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ArticleType];
                csubC.Length = EnumLibrary.PlaceLength;
                c.Criterias.Add(csubC);
            }
            if (string.IsNullOrEmpty(query.ModelName) || Constants.ArticleModelName.Equals(query.ModelName, StringComparison.OrdinalIgnoreCase))
            {
                AppendModelCondition(c);
            }
            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);
            if (query.ArticleType > 0)
                c.Add(CriteriaType.Equals, "ContentType", query.ArticleType);
            else
                c.Add(CriteriaType.NotEquals, "ContentType", 16);

            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "Title", "%"+query.KeyWord+"%");
                keyCriteria.AddOr(CriteriaType.Like, "Description", "%"+query.KeyWord+"%");
                c.Criterias.Add(keyCriteria);
            }

            if (!string.IsNullOrEmpty(query.Author))
                c.Add(CriteriaType.Like, "Author", "%"+query.Author+"%");
            if (query.BeginDate <= query.EndDate)
            {
                if (query.BeginDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.BeginDate);
                if (query.EndDate != DateTime.MinValue && query.EndDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.EndDate.AddDays(1));
            }
            else
            {
                if (query.EndDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.EndDate);
                if (query.BeginDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.BeginDate.AddDays(1));
            }

            if (!We7Helper.IsEmptyID(query.ChannelID))
            {
                if (CheckModel(modelname))
                {
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (!string.IsNullOrEmpty(parameters))
                    {
                        CriteriaExpressionHelper.Execute(c, parameters);
                    }
                }
                else
                {
                    if (query.ExcludeThisChannel)
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.NotLike, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                            c.Add(CriteriaType.NotEquals, "OwnerID", query.ChannelID);
                    }
                    else
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.Like, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                        {
                            if (query.ChannelID.Contains(","))
                            {
                                string[] oids = query.ChannelID.Split(',');
                                Criteria subC = new Criteria(CriteriaType.None);
                                subC.Mode = CriteriaMode.Or;
                                foreach (string oid in oids)
                                {
                                    subC.Add(CriteriaType.Equals, "OwnerID", oid);
                                }
                                c.Criterias.Add(subC);
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, "OwnerID", query.ChannelID);
                            }
                        }
                    }
                }
            }

            if (!We7Helper.IsEmptyID(query.AccountID))
            {
                Channel channel = ChannelHelper.GetChannel(query.ChannelID, null);
                if (query.IncludeAdministrable)
                {
                    List<string> channels = AccountHelper.GetObjectsByPermission(query.AccountID, "Channel.Article");

                    Criteria keyCriteria = new Criteria(CriteriaType.None);
                    if (channels != null && channels.Count > 0)
                    {
                        keyCriteria.Mode = CriteriaMode.Or;
                        foreach (string ownerID in channels)
                        {
                            keyCriteria.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                        }
                    }
                    keyCriteria.AddOr(CriteriaType.Equals, "AccountID", query.AccountID);
                    if (keyCriteria.Criterias.Count > 0)
                    {
                        c.Criterias.Add(keyCriteria);
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "AccountID", query.AccountID);
            }
            if (query.IsShowHome != null && query.IsShowHome == "1")
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (!string.IsNullOrEmpty(query.Tag))
            {
                c.Add(CriteriaType.Like, "Tags", "%"+query.Tag+"%");
            }
            if (query.IsImage != null && query.IsImage == "1")
            {
                c.Add(CriteriaType.Equals, "IsImage", 1);
            }
            if (query.Overdue)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                Criteria subChildC2 = new Criteria(CriteriaType.Equals, "Overdue", DateTime.MinValue);
                subC.Criterias.Add(subChildC2);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                c.Criterias.Add(subC);
            }
            if (!string.IsNullOrEmpty(query.ArticleParentID))
            {
                c.Add(CriteriaType.Equals, "ParentID", query.ArticleParentID);
            }
            //if (!string.IsNullOrEmpty(query.SearcherKey))

            if (!string.IsNullOrEmpty(query.ArticleID))
            {
                c.Add(CriteriaType.Like, "ListKeys", "%"+query.ArticleID+"%");
            }
            return c;
        }

        bool CheckModel(string modelname)
        {
            if (!string.IsNullOrEmpty(modelname))
            {
                modelname = modelname.ToLower();
                List<string> list = new List<string>() { "article", "system.article"};
                return !list.Contains(modelname);
            }
            return false;
        }
    }

    public class CriteriaExpressionHelper
    {
        static List<CriteriaExpression> expList = new List<CriteriaExpression>();

        static CriteriaExpressionHelper()
        {
            expList.Add(new LikeExpression());
        }

        public static void Execute(Criteria c, string expr)
        {
            foreach (CriteriaExpression exp in expList)
            {
                exp.Execute(c, expr);
            }
        }
    }

    public interface CriteriaExpression
    {
        void Execute(Criteria c, string expr);
    }

    public class LikeExpression : CriteriaExpression
    {
        Regex regex = new Regex(@"like\((?<field>\S*),(?<value>\S*)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Execute(Criteria c, string expr)
        {
            StringReader sr = new StringReader(expr);
            string s = null;
            while (!string.IsNullOrEmpty(s = sr.ReadLine()))
            {
                s = s.Trim();
                Match m = regex.Match(s);
                if (m != null || m.Success)
                {
                    c.Add(CriteriaType.Like, m.Groups["field"].Value, m.Groups["value"].Value);
                }
            }
        }
    }
}