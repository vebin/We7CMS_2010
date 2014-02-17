using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class Comments
    {
        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        string articleID;

        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }
        string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        string author;

        public string Author
        {
            get { return author; }
            set { author = value; }
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
        int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }
        int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        string timeNote;

        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }
        string ip;

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        string accountID;

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        string articleName;

        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }
        string articleTitle;

        public string ArticleTitle
        {
            get { return articleTitle; }
            set { articleTitle = value; }
        }

        public string AuditText
        {
            get
            {
                switch (State)
                { 
                    case 0:
                        return "<font color=red>已屏蔽</font>";
                    default:
                    case 1:
                        return "<font color=green>已启用</font>";
                    
                }
            }
        }


        #region Extension
        string _userSatisfaction;

        public string UserSatisfaction
        {
            get { return _userSatisfaction; }
            set { _userSatisfaction = value; }
        }
        string _contentCategory;

        public string ContentCategory
        {
            get { return _contentCategory; }
            set { _contentCategory = value; }
        }
        string _contentId;

        public string ContentID
        {
            get { return _contentId; }
            set { _contentId = value; }
        }
        #endregion

        
    }
}
