using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinkment.Data
{
    [Serializable]
    public class ObjectAssistant
    {
        Dictionaries _d1;

        public ObjectAssistant()
        {
            _d1 = new Dictionaries();
        }

        public void LoadDBConnectionString(string connectStr, string dbDriver)
        {
            _d1.SetGlobalDBString(connectStr, dbDriver);
        }

        public void LoadDataSource(string dir)
        {
            _d1.LoadDataSource(dir, null);
        }
    }
}
