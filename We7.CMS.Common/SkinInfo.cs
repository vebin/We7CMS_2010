using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7;
using System.Xml;
using System.IO;

namespace We7.CMS.Common
{
    [Serializable]
    public class SkinInfo : IXml
    {
        public SkinInfo()
        {
            items = new List<SkinItem>();
        }

        string basePath;
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        string ver;
        public string Ver
        {
            get { return ver; }
            set { ver = value; }
        }

        DateTime created = DateTime.Now;
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        List<SkinItem> items;
        private List<SkinItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public void ToFile(string basepath, string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(dec);
            doc.AppendChild(this.ToXml(doc));
            doc.Save(Path.Combine(basepath, filename));
        }

        [Serializable]
        public class SkinItem : IXml
        {
            string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            string description;
            public string Description
            {
                get { return description; }
                set { description = value; }
            }
            string template;
            public string Template
            {
                get { return template; }
                set { template = value; }
            }
            string templateText;
            public string TemplateText
            {
                get { return templateText; }
                set { templateText = value; }
            }
            string location;
            public string Location
            {
                get { return location; }
                set { location = value; }
            }
            string c_model;
            public string C_model
            {
                get { return c_model; }
                set { c_model = value; }
            }
            string layout;
            public string Layout
            {
                get { return layout; }
                set { layout = value; }
            }
            string tag;
            public string Tag
            {
                get { return tag; }
                set { tag = value; }
            }
            string type;
            public string Type
            {
                get { return type; }
                set { type = value; }
            }
            string c_modelText;
            public string C_modelText
            {
                get { return c_modelText; }
                set { c_modelText = value; }
            }
            bool isSubTemplate;
            public bool IsSubTemplate
            {
                get { return isSubTemplate; }
                set { isSubTemplate = value; }
            }
            string locationText;
            public string LocationText
            {
                get { return locationText; }
                set { locationText = value; }
            }
            DateTime created = DateTime.Now;
            public DateTime Created
            {
                get { return created; }
                set { created = value; }
            }
            DateTime updated = DateTime.Now;
            public DateTime Updated
            {
                get { return updated; }
                set { updated = value; }
            }


            public XmlElement ToXml(XmlDocument doc)
            {
                XmlElement xe = doc.CreateElement("Item");
                xe.SetAttribute("name", Name);
                xe.SetAttribute("description", Description);
                xe.SetAttribute("template", template);
                xe.SetAttribute("c_model", c_model);
                xe.SetAttribute("location", Location);
                xe.SetAttribute("layout", Layout);
                xe.SetAttribute("tag", Tag);
                xe.SetAttribute("type", Type);
                xe.SetAttribute("created", Created.ToString());
                xe.SetAttribute("updated", Updated.ToString());
                xe.SetAttribute("isSub", IsSubTemplate?Boolean.TrueString: Boolean.FalseString);
                xe.SetAttribute("c_modelText", C_modelText);
                xe.SetAttribute("locationText", LocationText);
                return xe;
            }

            public IXml FromXml(XmlElement xe)
            {
                Name = xe.GetAttribute("name");
                Description = xe.GetAttribute("description");
                Template = xe.GetAttribute("template");
                Layout = xe.GetAttribute("layout");
                Type = xe.GetAttribute("type");
                Location = xe.GetAttribute("location");
                C_model = xe.GetAttribute("c_model");
                Tag = xe.GetAttribute("tag");
                if (xe.GetAttribute("created") != null && xe.GetAttribute("created").Trim() != "")
                    Created = Convert.ToDateTime(xe.GetAttribute("created"));
                if (xe.GetAttribute("updated") != null && xe.GetAttribute("updated").Trim() != "")
                    Updated = Convert.ToDateTime(xe.GetAttribute("updated"));
                IsSubTemplate = xe.GetAttribute("isSub") == Boolean.TrueString;
                C_modelText = xe.GetAttribute("c_modelText");
                LocationText = xe.GetAttribute("locationText");

                return this;
            }
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("TemplateGroup");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("ver", Ver);
            xe.SetAttribute("created", Created.ToString());
            foreach (SkinItem item in Items)
            {
                xe.AppendChild(item.ToXml(doc));
            }
            return xe;
        }

        public IXml FromXml(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            Created = Convert.ToDateTime(xe.GetAttribute("created"));
            Ver = xe.GetAttribute("ver");
            foreach (XmlElement node in xe.SelectNodes("Item"))
            {
                SkinItem item = new SkinItem();
                item.FromXml(node);
                items.Add(item);
            }
            return this;
        }
    }
}
