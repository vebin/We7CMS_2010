using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.PageViewReportHelper")]
    public class PageViewReportHelper : BaseHelper
    {
        public StatisticsArticle GetStatisticsArticleCount()
        {
            StatisticsArticle sa = new StatisticsArticle();

            sa.TotalArticles = Assistant.Count<Article>(null);
            sa.TotalComments = Assistant.Count<Comments>(null);

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime thisMonth = Convert.ToDateTime(year.ToString()+"-"+month.ToString()+"-01");
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", thisMonth);

            sa.MonthArticles = Assistant.Count<Article>(c);
            sa.MonthComments = Assistant.Count<Comments>(c);

            DateTime currentTime = DateTime.Now;
            string currentWeek = currentTime.DayOfWeek.ToString();
            int dayCount = DayInWeek(currentWeek);
            DateTime thisWeek = Convert.ToDateTime(currentTime.ToShortDateString()).AddDays(-(dayCount-1));
            c = new Criteria(CriteriaType.MoreThanEquals, "Created", thisWeek);

            sa.WeekArticles = Assistant.Count<Article>(c);
            sa.WeekComments = Assistant.Count<Comments>(c);

            return sa;
        }

        public int DayInWeek(string week)
        {
            int daycount = 0;
            switch (week)
            { 
                case "Monday":
                    daycount = 1;
                    break;
                case "Tuesday":
                    daycount = 2;
                    break;
                case "Wednesday":
                    daycount = 3;
                    break;
                case "Thursday":
                    daycount = 4;
                    break;
                case "Friday":
                    daycount = 5;
                    break;
                case "Saturday":
                    daycount = 6;
                    break;
                case "Sunday":
                    daycount = 7;
                    break;
            }
            return daycount;
        }
    }
}
