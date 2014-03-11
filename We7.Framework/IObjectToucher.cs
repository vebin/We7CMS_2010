using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.Framework
{
    public interface IObjectToucher<T> : IFactable
    {
        void Touch(T obj);
    }
}
