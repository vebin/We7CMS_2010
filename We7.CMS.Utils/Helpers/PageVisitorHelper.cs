using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using Thinkment.Data;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.PageVisitorHelper")]
    public class PageVisitorHelper : BaseHelper
    {
        public const string VisiteCountCacheKey = "$WE7_VISITECOUNT";//访问缓存Key
        public static readonly string OnlinePeopleApplicationKey = "We7.Application.OnlinePeople.Key";
        public static readonly string PageVisitorSessionKey = "We7.Session.PageVisitor.Key";

        public object CopyPropertiesTo(object source, object target)
        {
            PropertyInfo[] infos = target.GetType().GetProperties();
            foreach (PropertyInfo p in infos)
            {
                if (p.CanWrite)
                    p.SetValue(target, p.GetValue(source, null), null);
            }
            return target;
        }

        public VisiteCount GetCurrentVisiteCount()
        {
            VisiteCount vc = AppCtx.Cache.RetrieveObject<VisiteCount>(VisiteCountCacheKey);
            if (vc == null || vc.CreateDate.Day != DateTime.Now.Day)
            {
                vc = null;
                AppCtx.Cache.RemoveObject(VisiteCountCacheKey);
            }
            if (vc == null)
            {
                DateTime oldest = GetOldestTime();
                if (oldest < DateTime.Today)
                {
                    MigrateToHistory();
                    FreshSumData(oldest);
                }
                vc = new VisiteCount();
                HttpContext context = HttpContext.Current;
                //vc.OnlineVisitors = (int)Context.Application[PageVisitorHelper.OnlinePeopleApplicationKey];
                Criteria c = new Criteria(CriteriaType.MoreThanEquals, "OnlineTime", DateTime.Now.AddMinutes(-15));
                vc.OnlineVisitors = Assistant.Count<PageVisitor>(c);

                //今日访问量
                vc.DayVisitors = Assistant.Count<PageVisitor>(null);
                vc.DayPageview = Assistant.Count<Statistics>(null);

                //总访问数
                vc.TotalVisitors = Assistant.Count<PageVisitorHistory>(null) + vc.DayVisitors;
                //总浏览量
                vc.TotalPageView = Assistant.Count<StatisticsHistory>(null) + vc.DayPageview;
                //今年访问量
                int year = DateTime.Now.Year;
                DateTime thisYear = Convert.ToDateTime(year.ToString() + "-01-01");
                c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", thisYear);
                vc.YearVisitors = Assistant.Count<PageVisitorHistory>(c) + vc.DayVisitors;
                //今年浏览量
                vc.YearPageview = Assistant.Count<StatisticsHistory>(c) + vc.DayPageview;
                //本月访问量
                int month = DateTime.Now.Month;
                DateTime thisMonth = Convert.ToDateTime(year.ToString()+"-"+month.ToString()+"-01");
                c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", thisMonth);
                vc.MonthVisitors = Assistant.Count<PageVisitorHistory>(c) + vc.DayVisitors;
                vc.MonthPageview = Assistant.Count<StatisticsHistory>(c) + vc.DayPageview;
                //昨天访问量
                c = new Criteria(CriteriaType.LessThan, "VisitDate", DateTime.Today);
                Criteria subc = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", DateTime.Today.AddDays(-1));
                c.Criterias.Add(subc);
                vc.YestodayVisitors = Assistant.Count<PageVisitorHistory>(c);
                vc.YestodayPageview = Assistant.Count<StatisticsHistory>(c);

                //平均每天访问量
                Order[] o = new Order[] { new Order("VisitDate", OrderMode.Asc) };
                List<PageVisitorHistory> list = Assistant.List<PageVisitorHistory>(null, o, 0, 1);
                DateTime firstDay = list.Count>0 ? list[0].VisitDate : DateTime.Now;
                vc.StartDate = firstDay;
                int days = ((TimeSpan)(DateTime.Today - firstDay.Date)).Days + 1;

                if (days > 0)
                {
                    vc.AverageDayVisitors = vc.TotalVisitors / days;
                    vc.AverageDayPageview = vc.TotalPageView / days;
                }
                AppCtx.Cache.AddObject(VisiteCountCacheKey, vc, ((int)CacheTime.Short)*1000);
            }
            return vc;
        }

        public DateTime GetLogoutDate(string visitorId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            List<StatisticsHistory> list = Assistant.List<StatisticsHistory>(c, o);
            if (list.Count > 0)
                return list[0].VisitDate;
            else
                return DateTime.MinValue;
        }

        public List<PageVisitorHistory> GetPageVisitors(Criteria c)
        { 
            Order[] o = new Order[]{new Order("VisitDate", OrderMode.Desc)};
            return Assistant.List<PageVisitorHistory>(c, o);
        }

        public int GetStatisticsCount(string visitorId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            return Assistant.Count<StatisticsHistory>(c);
        }

        public DateTime GetOldestTime()
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Asc) };
            List<PageVisitor> list = Assistant.List<PageVisitor>(null, o, 0, 1);
            if (list != null && list.Count > 0)
            {
                return list[0].VisitDate;
            }
            else
                return DateTime.Today;
        }

        public void MigrateToHistory()
        {
            Criteria c = new Criteria(CriteriaType.LessThan, "VisitDate", DateTime.Today);
            List<PageVisitor> list = Assistant.List<PageVisitor>(c, null);
            foreach (PageVisitor o in list)
            {
                PageVisitorHistory pvh = new PageVisitorHistory();
                pvh = CopyPropertiesTo((object)o, (object)pvh) as PageVisitorHistory;

                try
                {
                    Assistant.Insert(pvh, null);
                }
                catch
                {
                    continue;
                }
            }

            List<Statistics> list2 = Assistant.List<Statistics>(c, null);
            foreach (Statistics s in list2)
            {
                StatisticsHistory sh = new StatisticsHistory();
                sh = CopyPropertiesTo((object)s, (object)sh) as StatisticsHistory;

                try
                {
                    Assistant.Insert(sh, null);
                }
                catch
                { 
                    continue;
                }
            }
            Assistant.DeleteList<Statistics>(c);
            Assistant.DeleteList<PageVisitor>(c);
        }

        public void FreshSumData(DateTime startTime)
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", startTime);
            List<PageVisitorHistory> list = GetPageVisitors(c);
            int count = 0;
            foreach (PageVisitorHistory pvh in list)
            {
                bool needUpdate = false;
                if (pvh.Clicks <= 1)
                {
                    pvh.Clicks = GetStatisticsCount(pvh.ID);
                    if (pvh.Clicks > 1) needUpdate = true;
                }
                pvh.PageView = pvh.Clicks;
                if (string.IsNullOrEmpty(pvh.City))
                {
                    string[] parts = We7Helper.GetCityNameByIP(pvh.VisitorIP).Split(' ');
                    if (parts.Length > 0)
                        pvh.Province = parts[0];
                    if (parts.Length > 1)
                        pvh.City = parts[1];
                    needUpdate = true;
                }
                if (string.IsNullOrEmpty(pvh.FromSite))
                {
                    pvh.FromSite = We7Helper.GetDomainFromUrl(pvh.HttpReferer);
                    needUpdate = true;
                }
                if (pvh.VisitDate == pvh.LogoutDate)
                {
                    pvh.LogoutDate = GetLogoutDate(pvh.ID);
                    pvh.OnlineTime = pvh.LogoutDate;
                    needUpdate = true;
                }

                if (needUpdate)
                {
                    try
                    {
                        UpdatePageVisitor(pvh, new string[]{"City", "Province", "FromSite", "Clicks", "LogoutDate", "OnlineTime"});
                    }
                    finally
                    {
                        count += 1;
                    }
                }
            }
        }

        public void UpdatePageVisitor(PageVisitorHistory pvh, string[] fields)
        {
            Assistant.Update(pvh, fields);
        }

        public void PageVisitorLeave()
        {
            if (HttpContext.Current != null)
            {
                HttpSessionState session = HttpContext.Current.Session;
                if (session[PageVisitorHelper.PageVisitorSessionKey] != null)
                {
                    PageVisitor pv = (PageVisitor)session[PageVisitorHelper.PageVisitorSessionKey];
                    pv.OnlineTime = DateTime.Now;
                    Assistant.Update(pv, new string[]{"OnlineTime"});
                }
            }
        }
    }
}
