using System;
using System.Collections.Generic;
using System.Linq;
using We7.CMS.Common.Enum;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class Channel
    {
        public static int MaxLevels = 8;

        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        string parentID;

        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }
        string alias;

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        string templateName;

        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }
        string detailTemplate;

        public string DetailTemplate
        {
            get { return detailTemplate; }
            set { detailTemplate = value; }
        }
        int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public string StateText
        {
            get { return State == 0 ? "不可用" : "可用"; }    
        }

        int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        int securityLevel;

        public int SecurityLevel
        {
            get { return securityLevel; }
            set { securityLevel = value; }
        }

        public string SecurityLevelText
        {
            get 
            {
                switch (SecurityLevel)
                { 
                    case 1:
                        return "中";
                    case 2:
                        return "高";
                    default:
                    case 0:
                        return "低";
                }
            }
        }

        string referenceID;

        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
        }
        string parameter;

        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }
        DateTime created = DateTime.Now;

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        string fullPath;

        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }
        string templateText;

        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }
        string detailTemplateText;

        public string DetailTemplateText
        {
            get { return detailTemplateText; }
            set { detailTemplateText = value; }
        }
        List<Channel> channels;

        public List<Channel> Channels
        {
            get { return channels; }
            set { channels = value; }
        }
        string defaultContentID;

        public string DefaultContentID
        {
            get { return defaultContentID; }
            set { defaultContentID = value; }
        }
        string channelFolder;

        public string ChannelFolder
        {
            get { return channelFolder; }
            set { channelFolder = value; }
        }
        string titleImage;

        public string TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; }
        }
        string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string TypeText
        {
            get
            {
                switch ((TypeOfChannel)int.Parse(Type))
                { 
                    case TypeOfChannel.QuoteChannel:
                        return "专题型";
                    case TypeOfChannel.RssOriginal:
                        return "RSS源";
                    case TypeOfChannel.BlankChannel:
                        return "空节点";
                    case TypeOfChannel.ReturnChannel:
                        return "跳转型";
                    default:
                    case TypeOfChannel.NormalChannel:
                        return "原创型";
                }
            }
        }

        string process;

        public string Process
        {
            get { return process; }
            set { process = value; }
        }
        string channelName;

        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }
        string refAreaID;

        public string RefAreaID
        {
            get { return refAreaID; }
            set { refAreaID = value; }
        }
        int isComment;

        public int IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }

        public string IsCommentText
        {
            get
            {
                switch (isComment)
                { 
                    case 1:
                        return "允许登录用户评论";
                    case 2:
                        return "允许匿名评论";
                    default:
                    case 0:
                        return "不允许评论";
                }
            }
        }
        
        string fullUrl;

        public string FullUrl
        {
            get 
            {
                if (isOldFullUrl)
                {
                    return fullUrl;
                }
                else
                {
                    if (fullUrl != null)
                    {
                        string cleanUrl = fullUrl;
                        while (cleanUrl.StartsWith("/"))
                            cleanUrl = cleanUrl.Remove(0, 1);
                        if (cleanUrl.EndsWith("/"))
                            cleanUrl.Remove(cleanUrl.Length - 1);
                        cleanUrl = "/" + cleanUrl + "/";
                        return cleanUrl;
                    }
                    else
                        return fullUrl;
                }
            }
            set { fullUrl = value; }
        }
        string returnUrl;

        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }
        string processLayerNO;

        public string ProcessLayerNO
        {
            get { return processLayerNO; }
            set { processLayerNO = value; }
        }
        DateTime updated = DateTime.Now;

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public string ModelName { get; set; }

        string enumState;

        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }
        int articlesCount;

        public int ArticlesCount
        {
            get { return articlesCount; }
            set { articlesCount = value; }
        }
        string tags;

        public string Tags
        {
            get 
            {
                if (tags == null)
                    return string.Empty;
                else
                    return tags;
            }
            set { tags = value; }
        }
        string keyWord;

        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }
        string descriptionKey;

        public string DescriptionKey
        {
            get { return descriptionKey; }
            set { descriptionKey = value; }
        }
        string ipstrategy;

        public string IPStrategy
        {
            get { return ipstrategy; }
            set { ipstrategy = value; }
        }
        bool isOldFullUrl;

        public bool IsOldFullUrl
        {
            get { return isOldFullUrl; }
            set { isOldFullUrl = value; }
        }

    }
}
