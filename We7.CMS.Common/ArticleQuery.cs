using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    [Serializable]
    public class ArticleQuery
    {
        string accountID;

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        string channelID;

        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        public string ChannelFullUrl { get; set; }

        bool excludeThisChannel = false;

        public bool ExcludeThisChannel
        {
            get { return excludeThisChannel; }
            set { excludeThisChannel = value; }
        }
        string keyWord;

        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }
        DateTime beginDate;

        public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        int articleType;

        public int ArticleType
        {
            get { return articleType; }
            set { articleType = value; }
        }
        ArticleStates currentState = ArticleStates.All;

        public ArticleStates State
        {
            get { return currentState; }
            set { currentState = value; }
        }
        bool includeAllSons;

        public bool IncludeAllSons
        {
            get { return includeAllSons; }
            set { includeAllSons = value; }
        }
        bool includeAdministrable = false;

        public bool IncludeAdministrable
        {
            get { return includeAdministrable; }
            set { includeAdministrable = value; }
        }
        string enumState;

        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }
        string author;

        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        string orderKeys = "Updated|Desc";

        public string OrderKeys
        {
            get { return orderKeys; }
            set { orderKeys = value; }
        }
        bool orderDesc;

        public bool OrderDesc
        {
            get { return orderDesc; }
            set { orderDesc = value; }
        }
        string isShowHome;

        public string IsShowHome
        {
            get { return isShowHome; }
            set { isShowHome = value; }
        }
        string listKeys;

        public string ListKeys
        {
            get { return listKeys; }
            set { listKeys = value; }
        }
        string listKeys2;

        public string ListKeys2
        {
            get { return listKeys2; }
            set { listKeys2 = value; }
        }
        string modelName;

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        string tag = "";

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        bool overdue = false;

        public bool Overdue
        {
            get { return overdue; }
            set { overdue = value; }
        }

        string articleID;

        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        string isImage;

        public string IsImage
        {
            get { return isImage; }
            set { isImage = value; }
        }

        public bool UseModel { get; set; }

        public string ArticleParentID { get; set; }
    }
}
