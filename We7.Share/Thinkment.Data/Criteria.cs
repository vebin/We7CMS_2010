using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public class Criteria
    {
        List<Criteria> criterias;
        public List<Criteria> Criterias
        {
            get { return criterias; }
        }

        CriteriaMode mode;
        public CriteriaMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        CriteriaType type;
        public CriteriaType Type
        {
            get { return type; }
            set { type = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        Adorns adorn;
        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }

        int start;
        public int Start
        {
            get { return start; }
            set { start = value; }
        }

        int length;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public Criteria()
        {
            criterias = new List<Criteria>();
            mode = CriteriaMode.And;
            type = CriteriaType.None;
        }

        public Criteria(CriteriaType t)
            : this()
        {
            type = t;
        }

        public Criteria(CriteriaType t, string n, object v)
            : this(t)
        {
            name = n;
            value = v;
        }

        public void AddOr(CriteriaType type, string name, object value)
        {
            Add(type, CriteriaMode.Or, name, value);
        }

        public void Add(CriteriaType type, string name, object value)
        {
            Add(type, CriteriaMode.And, name, value);
        }

        void Add(CriteriaType type, CriteriaMode mode, string name, object value)
        {
            Criteria c = new Criteria(type, name, value);
            c.Mode = mode;
            c.Criterias.Add(c);
        }
    }
}
