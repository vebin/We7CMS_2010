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
        CriteriaMode mode;
        CriteriaType type;
        string name;
        object value;

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
    }
}
