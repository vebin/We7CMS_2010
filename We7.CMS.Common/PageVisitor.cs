using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class PageVisitor
    {
        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        int typeCode;

        public int TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }
        string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        DateTime visitDate;

        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }
        DateTime logoutDate;

        public DateTime LogoutDate
        {
            get { return logoutDate; }
            set { logoutDate = value; }
        }
        string visitorIP;

        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }
        string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        string http_referer;

        public string HttpReferer
        {
            get { return http_referer; }
            set { http_referer = value; }
        }
        string searchEngine;

        public string SearchEngine
        {
            get { return searchEngine; }
            set { searchEngine = value; }
        }
        string keyword;

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }
        int clicks;

        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }
        DateTime onlineTime;

        public DateTime OnlineTime
        {
            get { return onlineTime; }
            set { onlineTime = value; }
        }
        string platform;

        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }
        string browser;

        public string Browser
        {
            get { return browser; }
            set { browser = value; }
        }
        string screen;

        public string Screen
        {
            get { return screen; }
            set { screen = value; }
        }
        string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }
        int pageView;

        public int PageView
        {
            get { return pageView; }
            set { pageView = value; }
        }
        DateTime updated = DateTime.Now;

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        DateTime created = DateTime.Now;

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        string fromSite;

        public string FromSite
        {
            get { return fromSite; }
            set { fromSite = value; }
        }
        string province;

        public string Province
        {
            get { return province; }
            set { province = value; }
        }
    }

    public class VisiteCount : ICloneable
    {
        private int totalpageview;

        public int TotalPageView
        {
            get { return totalpageview; }
            set { totalpageview = value; }
        }
        private int totalVisitors;

        public int TotalVisitors
        {
            get { return totalVisitors; }
            set { totalVisitors = value; }
        }
        private int yearVisitors;

        public int YearVisitors
        {
            get { return yearVisitors; }
            set { yearVisitors = value; }
        }
        private int monthVisitors;

        public int MonthVisitors
        {
            get { return monthVisitors; }
            set { monthVisitors = value; }
        }
        private int dayVisitors;

        public int DayVisitors
        {
            get { return dayVisitors; }
            set { dayVisitors = value; }
        }
        private int yestodayVisitors;

        public int YestodayVisitors
        {
            get { return yestodayVisitors; }
            set { yestodayVisitors = value; }
        }
        private int averageDayVisitors;

        public int AverageDayVisitors
        {
            get { return averageDayVisitors; }
            set { averageDayVisitors = value; }
        }
        private int yearPageview;

        public int YearPageview
        {
            get { return yearPageview; }
            set { yearPageview = value; }
        }
        private int monthPageview;

        public int MonthPageview
        {
            get { return monthPageview; }
            set { monthPageview = value; }
        }
        private int dayPageview;

        public int DayPageview
        {
            get { return dayPageview; }
            set { dayPageview = value; }
        }
        private int yestodayPageview;

        public int YestodayPageview
        {
            get { return yestodayPageview; }
            set { yestodayPageview = value; }
        }
        private int averageDayPageview;

        public int AverageDayPageview
        {
            get { return averageDayPageview; }
            set { averageDayPageview = value; }
        }
        private int onlineVisitors;

        public int OnlineVisitors
        {
            get { return onlineVisitors; }
            set { onlineVisitors = value; }
        }

        private DateTime startDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime CreateDate { get; set; }

        #region ICloneable 成员
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        public VisiteCount Clone()
        {
            return this.MemberwiseClone() as VisiteCount;
        }
            
        #endregion
    }

    public class PageVisitorHistory : PageVisitor
    {
        public PageVisitorHistory() { }
    }
}
