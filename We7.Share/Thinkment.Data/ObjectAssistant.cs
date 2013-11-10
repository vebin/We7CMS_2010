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

        public int Count<T>(Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return Count<T>(conn, condition);
            }
        }

        public int Count<T>(IConnection conn, Criteria condition)
        {
            ObjectManager om = _d1.GetObjectManager(typeof(T));
            return om.MyCount(conn, condition);
        }
    }
}
