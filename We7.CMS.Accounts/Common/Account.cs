using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Common.PF
{
    [Serializable]
    public class Account
    {
        public Account()
        {
            PasswordHashed = 1;
            Password = "111111";
            State = 1;
            Overdue = DateTime.Today.AddYears(10);
        }

        public int EmailValidate { get; set; }

        public string ID { get; set; }

        public DateTime Overdue { get; set; }

        public string Password { get; set; }

        public int PasswordHashed { get; set; }

        public int State { get; set; }

    }
}
