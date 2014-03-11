using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS
{
    public interface ITemplateService
    {

        void Initial();
    }

    public class TemplateServiceFactory
    {
        private static DefaultTemplateService _defaultTemplateService;

        public static ITemplateService Create()
        {
            if (_defaultTemplateService == null)
            {
                _defaultTemplateService = new DefaultTemplateService();
            }

            return _defaultTemplateService;
        }
    }
}
