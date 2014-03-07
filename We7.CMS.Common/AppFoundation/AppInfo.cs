using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.AppFoundation
{
    [Serializable]
    public class AppInfo
    {
        public AppInfo(AppMetadata appMetadata)
        {
            Metadata = appMetadata;
        }

        public AppInfo(AppMetadata appMetadata, bool isEnable)
        {
            Metadata = appMetadata;
            IsEnable = isEnable;
        }

        public string Id
        {
            get { return (null == Metadata) ? null : Metadata.Id; }
        }

        public bool IsEnable { get; internal set; }

        public AppMetadata Metadata { get; private set; }
    }
}
