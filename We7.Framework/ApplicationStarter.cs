using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using We7.Framework.Factable;

namespace We7.Framework
{

    public interface IWe7CmsStarter
    {
        void Start(HttpApplication application);

        IContainer CreateContainer();
    }

    public class ApplicationStarter : IWe7CmsStarter
    {
        readonly string[] _preLoad =
            new[]{
                "We7.Framework",
                "We7.CMS.Accounts",
                "We7.CMS.Common",
                "We7.Model.Core",
                "We7.CMS.Utils",
                "We7.CMS.Install",
                "We7.UrlRewriter"
            };

        public void Start(HttpApplication application)
        {
            IContainer container = CreateContainer();
            ContainerContext.SetContainer(container, application as IAppContainerHolder);
        }

        public IContainer CreateContainer()
        {
            return new Container(new ContainerStorage());
        }
    }
}
