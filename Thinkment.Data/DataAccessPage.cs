using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Thinkment.Data
{
    public interface IDataAccessPage
    { 
        
    }

    internal sealed class DBAccessHelper
    {
        public static bool IsDataAccessPage
        {
            get
            { 
                return HttpContext.Current != null && HttpContext.Current.Items != null &&
                    HttpContext.Current.Handler != null && HttpContext.Current.Handler is IDataAccessPage &&
                    HttpContext.Current.Application["isMulti"] == null;
            }
        }
    }
}
