using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public class Order
    {
        Adorns adorn;
        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }

        OrderMode mode;
        public OrderMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
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

        string aliasName;
        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }

        public Order()
        {
            adorn = Adorns.None;
        }

        public Order(string n)
            : this()
        {
            name = n;
            mode = OrderMode.Asc;
        }

        public Order(string n, OrderMode m)
            : this()
        {
            name = n;
            mode = m;
        }

    }
}
