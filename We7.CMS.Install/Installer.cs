using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Config;
using System.IO;
using Thinkment.Data;

namespace We7.CMS.Install
{
    public static class Installer
    {
        public static bool CheckConnection(BaseConfigInfo bci, out string msg)
        {
            msg = "";
            try
            {
                string connectionString = bci.DBConnectionString;
                string selectDbType = bci.DBType;
                connectionString = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                IDbDriver driver = CreateDbDriver(selectDbType);
                using (IConnection conn = driver.CreateConnection(connectionString))
                {
                    SqlStatement st = new SqlStatement("SELECT 1");
                    conn.QueryScalar(st);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Trim().ToUpper().StartsWith("ORA-00923"))
                    return true;
                else
                {
                    msg = ex.Message;
                    return false;
                }
            }
        }

        public static int CreateDatabase(BaseConfigInfo bci, out Exception resultException)
        {
            int result = 0; //数据库新建失败
            resultException = null;
            DatabaseInfo dbi = GetDatabaseInfo(bci);
            string dbFile = "";
            if (dbi.DBFile != null && dbi.DBFile != "")
            {
                dbFile = dbi.DBFile.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                dbFile.Replace('\\', Path.DirectorySeparatorChar);
            }

            try
            {
                switch (bci.DBType)
                {
                    case "SqlServer":
                        string masterstring = string.Format(@"Server={0};Database={1};User={2};Password={3};", dbi.Server, "master", dbi.User, dbi.Password);
                        string sql = string.Format(@"IF NOT EXISTS (SELECT * FROM SYSDATABASES WHERE NAME=N'{0}') CREATE DATABASE {0}", dbi.Database);
                        IDbDriver driver = new SqlDbDriver();

                        using (IConnection conn = driver.CreateConnection(masterstring))
                        {
                            SqlStatement st0 = new SqlStatement(string.Format("SELECT count(*) FROM SYSDATABASES WHERE NAME=N'{0}'", dbi.Database));
                            int count = (int)conn.QueryScalar(st0);
                            if (count == 0)
                            {
                                SqlStatement st = new SqlStatement(sql);
                                driver.FormatSQL(st);
                                driver.FormatSQL(st0);
                                conn.Update(st);
                                if ((int)conn.QueryScalar(st0) > 0)
                                    result = 1; //代表数据库新建成功
                            }
                            else
                                result = -1;    //代表数据库已经存在，无需新建
                        }
                        break;

                    case "MySql":
                        result = -1;
                        break;
                    case "Oracle":
                        result = -1;
                        break;
                    case "SQLite":
                        if (File.Exists(dbFile))
                            result = -1;
                        else
                        {
                            string dbPath = Path.GetDirectoryName(dbFile);
                            if (!Directory.Exists(dbPath))
                                Directory.CreateDirectory(dbPath);
                            System.Data.SQLite.SQLiteConnection.CreateFile(dbFile);
                            if (File.Exists(dbFile))
                                result = 1;
                        }
                        break;
                    case "Access":
                        if (!File.Exists(dbFile))
                            result = -1;
                        else
                        {
                            string dbPath = Path.GetDirectoryName(dbFile);
                            if (!Directory.Exists(dbPath))
                                Directory.CreateDirectory(dbPath);
                            ADOX.Catalog catalog = new ADOX.Catalog();
                            catalog.Create(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5;", dbFile));
                            if (File.Exists(dbFile))
                                result = 1;
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                resultException = ex;
                result = 0;
            }

            return result;
        }

        public static IDbDriver CreateDbDriver(string dbType)
        {
            IDbDriver driver = null;
            switch (dbType.ToLower())
            { 
                case "sqlserver":
                    driver = new SqlDbDriver();
                    break;
                case "mysql":
                    driver = new MySqlDriver();
                    break;
                case "oracle":
                    driver = new OracleDriver();
                    break;
                case "sqlite":
                    driver = new SQLiteDriver();
                    break;
                case "access":
                    driver = new OleDbDriver();
                    break;
            }

            return driver;
        }

        public static void ExecuteSQL(BaseConfigInfo bci, string file)
        {
            if (file != "" && File.Exists(file))
            { 
                
            }
        }

        public static void ExecuteSQLGroup(BaseConfigInfo bci)
        {
            List<string> files = new List<string>();
            files.Add("create.xml");
            files.Add("install.xml");
            files.Add("update.xml");
            files.Add("data.xml");
            ExecuteSQLGroup(bci, files);
        }

        public static void ExecuteSQLGroup(BaseConfigInfo bci, List<string> files)
        {
            string basepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install/SQL");
            string corepath = Path.Combine(basepath, "core");

            foreach (string file in files)
            {
                ExecuteSQL(bci, Path.Combine(corepath, file));
            }
        }

        public static DatabaseInfo GetDatabaseInfo(BaseConfigInfo bci)
        {
            DatabaseInfo dbi = new DatabaseInfo();
            string connectionstring = bci.DBConnectionString;
            string selectDBType = bci.DBType.ToLower() ;

            if (selectDBType == "sqlserver" || selectDBType == "mysql" || selectDBType == "oracle")
            {
                foreach (string info in connectionstring.Split(';'))
                {
                    if (info.ToLower().IndexOf("server") >= 0 || info.ToLower().IndexOf("data source") >= 0)
                    {
                        dbi.Server = info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("database") >= 0)
                    {
                        dbi.Database = info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("user") >= 0 || info.ToLower().IndexOf("uid") >= 0 || info.ToLower().IndexOf("user id") >= 0)
                    {
                        dbi.User = info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("password") >= 0 || info.ToLower().IndexOf("pwd") >= 0)
                    {
                        dbi.Password = info.Split('=')[1].Trim();
                        continue;
                    }
                }
            }
            else
            {
                foreach (string info in connectionstring.Split(';'))
                {
                    if (info.ToLower().IndexOf("data source") >= 0)
                    {
                        dbi.DBFile = info.Split('=')[1].Trim();
                    }
                }
            }

            return dbi;
        }

        public static BaseConfigInfo GenerateConnectionString(string selectDbType, DatabaseInfo dbi)
        {
            string dbDriver = string.Empty;
            string connectionstring = string.Empty;

            if (dbi.DBFile.IndexOf("\\") > -1)
                dbi.DBFile.Substring(dbi.DBFile.LastIndexOf("\\") + 1);
            string path = "{$App}\\App_Data\\DB\\" + dbi.DBFile;
            switch (selectDbType)
            { 
                case "SqlServer":
                    connectionstring = string.Format(@"Server={0};Database={1};User={2};Password={3};Pooling=True;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;",
                        dbi.Server, dbi.Database, dbi.User, dbi.Password);
                    dbDriver = "Thinkment.Data.SqlDbDriver";
                    break;
                case "MySql":
                    connectionstring = string.Format(@"server={0};database={1};uid={2};Pwd={3};charset=utf8;Pooling=true;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;",
                        dbi.Server, dbi.Database, dbi.User, dbi.Password);
                    dbDriver = "Thinkment.Data.MySqlDriver";
                    break;
                case "Oracle":
                    connectionstring = string.Format(@"Data Source={0};User ID={1};Password={2};Pooling=True;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;",
                        dbi.Server, dbi.User, dbi.Password);
                    dbDriver = "Thinkment.Data.OracleDriver";
                    break;
                case "SQLite":
                    connectionstring = string.Format(@"New=False;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={0};Pooling=True;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;", path);
                    dbDriver = "Thinkment.Data.SQLiteDriver";
                    break;
                case "Access":
                    connectionstring = string.Format(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=True;", path);
                    dbDriver = "Thinkment.Data.OleDbDriver";
                    break;
            }

            BaseConfigInfo baseConfig = new BaseConfigInfo();
            baseConfig.DBConnectionString = connectionstring;
            baseConfig.DBType = selectDbType;
            baseConfig.DBDriver = dbDriver;

            return baseConfig;
        }
    }
}
