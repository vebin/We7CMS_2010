using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Reflection;

namespace Thinkment.Data
{
    [Serializable]
    public class Property
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string field;
        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        DbType type;
        public DbType Type
        {
            get { return type; }
            set { type = value; }
        }

        int scale;
        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        bool nullable;
        public bool Nullable
        {
            get { return nullable; }
            set { nullable = value; }
        }

        bool _readonly;
        public bool Readonly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }

        PropertyInfo info;
        public PropertyInfo Info
        {
            get { return info; }
            set { info = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Property()
        {
            name = string.Empty;
            field = string.Empty;
            size = 10;
            scale = 0;
            type = DbType.String;
            nullable = true;
            description = string.Empty;
        }

        public Property FromXml(XmlElement element)
        {
            name = element.GetAttribute("name");
            field = element.GetAttribute("field");
            type = (DbType)Enum.Parse(typeof(DbType), element.GetAttribute("type"), true);
            size = UpdateXmlElement.GetXEAttribute(element, "size", 0);
            scale = UpdateXmlElement.GetXEAttribute(element, "scale", 0);
            nullable = UpdateXmlElement.GetXEAttribute(element, "nullable", false);
            _readonly = UpdateXmlElement.GetXEAttribute(element, "readonly", false);
            description = element.GetAttribute("description");
            return this;
        }
    }
}
