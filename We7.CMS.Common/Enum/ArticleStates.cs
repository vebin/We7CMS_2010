using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.Enum
{

    public enum ArticleStates
    {
        /// <summary>
        /// 已停用
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// 已启用
        /// </summary>
        Started = 1,

        /// <summary>
        /// 审核中
        /// </summary>
        Checking = 2,

        /// <summary>
        /// 已过期
        /// </summary>
        Overdued = 3,

        /// <summary>
        /// 回收站
        /// </summary>
        Recycled = -1,

        /// <summary>
        /// 全部
        /// </summary>
        All = 99
    }
}
