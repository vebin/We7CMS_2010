using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 访问统计类
    /// </summary>
    [Serializable]
    public class Statistics
    {
        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        //int typeCode;
        string visitorID;

        public string VisitorID
        {
            get { return visitorID; }
            set { visitorID = value; }
        }
        string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        string articleID;

        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }
        string channelID;

        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }
        string articleName;

        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }
        DateTime visitDate;

        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }
        string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        string visitorIP;

        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }
        string timeNote;

        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }
        DateTime created = DateTime.Now;

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        DateTime updated = DateTime.Now;

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
    }

    public class StatisticsHistory : Statistics
    {
        public StatisticsHistory() { }
    }
}
