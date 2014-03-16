using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common.AppFoundation;

namespace We7.CMS.Common.RequestFilters
{
    public class AbstractResultFilter : IResultFilter
    {

        public virtual void ResultExecuting(ResultExecutingContext context)
        {
        }

        public virtual void ResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
