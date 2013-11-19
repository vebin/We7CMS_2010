using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace We7
{
    public interface IXml
    {
        XmlElement ToXml(XmlDocument doc);

        IXml FromXml(XmlElement xe);
    }
}
