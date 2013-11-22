using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS
{
    [Serializable]
    [Helper("We7.LogHelper")]
    public class LogHelper : BaseHelper
    {
        public void WriteLog(string accountId, string page, string content, string remark)
        {
            Log log = new Log();
            log.UserID = accountId;
            log.Page = page;
            log.Content = content;
            log.Remark = remark;

            AddLog(log);
        }

        public void AddLog(Log log)
        {
            log.Created = DateTime.Now;
            log.ID = We7Helper.CreateNewID();
            Assistant.Insert(log);
        }
    }
}
