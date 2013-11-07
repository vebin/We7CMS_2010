using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Accounts
{
    public interface IAccountHelper
    {
        string[] Login(string username, string password);
    }
}
