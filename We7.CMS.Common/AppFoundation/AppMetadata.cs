using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.AppFoundation
{
    [Serializable]
    public abstract class AppMetadata
    {
        public abstract string Id { get; set;}

        public abstract string Logo { get; set; }

        public abstract AppMenuItem[] MenuItems { get; set; }
    }
}
