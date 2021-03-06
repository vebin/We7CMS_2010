﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class Article
    {
        public Article()
        {
            Created = DateTime.Now;
        }

        public string ID { get; set; }

        public DateTime Created { get; set; }

        public string AttachmentUrlPath
        {
            get 
            {
                string year = Created.ToString("yyyy");
                string month = Created.ToString("MM");
                string day = Created.ToString("dd");
                string sn = We7Helper.GUIDToFormatString(ID);

                return string.Format("/_data/{0}/{1}/{2}/{3}", year, month, day, sn);
            }
        }
    }

    public class StatisticsArticle
    {
        public int TotalArticles { get; set; }

        public int TotalComments { get; set; }

        public int MonthArticles { get; set; }

        public int MonthComments { get; set; }

        public int WeekArticles { get; set; }

        public int WeekComments { get; set; }
    }
}
