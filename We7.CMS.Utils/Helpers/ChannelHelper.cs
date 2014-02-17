using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using System.Web;
using Thinkment.Data;
using System.Web.Caching;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.")]
    public class ChannelHelper : BaseHelper
    {

        private static readonly string ChannelKeyID = "ChannelID{0}";

        public Channel GetChannel(string channelID, string[] fields)
        {
            if (!string.IsNullOrEmpty(channelID))
            {
                Channel channel = null;
                HttpContext Context = HttpContext.Current;
                if (Context != null)
                {
                    string key = string.Format(ChannelKeyID, channelID);
                    channel = (Channel)Context.Items[key];
                    if (channel == null)
                    {
                        channel = (Channel)Context.Cache[key];
                        if (channel == null)
                        {
                            if (!string.IsNullOrEmpty(channelID))
                            {
                                Order[] o = new Order[] { new Order("ID") };
                                Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                                List<Channel> channels = Assistant.List<Channel>(c, o);
                                if (channels.Count > 0)
                                {
                                    channel = channels[0];
                                }
                            }
                            if (channel != null)
                            {
                                CacherCache(key, Context, channel, CacheTime.Short);
                            }
                        }
                        if (Context.Items[key] == null)
                        {
                            Context.Items.Remove(key);
                            Context.Items.Add(key, channel);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(channelID))
                    {
                        Order[] o = new Order[] { new Order("ID") };
                        Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                        List<Channel> channels = Assistant.List<Channel>(c, o);
                        if (channels.Count > 0)
                        {
                            channel = channels[0];
                        }
                    }
                }
                return channel;
            }
            else
                return null;
        }
        public string GetModelName(string oid, out string Parameter)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                Parameter = ch.Parameter;
                return ch.ModelName;
            }
            else
            {
                Parameter = string.Empty;
                return string.Empty;
            }
        }


    }
}
