﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    public interface IDatabase
    {
        string Name { get; set; }
        string ConnectionString { get; set; }
    }
}
