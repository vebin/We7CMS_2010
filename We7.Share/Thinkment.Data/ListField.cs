using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public class ListField
    {
        public ListField()
        {
            adorn = Adorns.None;
        }

        public ListField(string fn)
            : this()
        {
            fieldName = fn;
        }

        private Adorns adorn;
        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }

        private string fieldName;
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
    }
}
