using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework
{
    public static class Enumerable
    {
        public static IEnumerable<T> Empty<T>()
        { 
            return new T[0];
        }
    }
}
