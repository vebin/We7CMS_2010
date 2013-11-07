using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using We7.Framework.Util;

namespace We7.Framework
{
    [Serializable]
    public class HelperFactory
    {
        Dictionary<Type, IHelper> helpers;
        List<Assembly> assemblies;
        string root;

        public string Root
        {
            get { return root; }
            set { root = value; }
        }

        public HelperFactory()
        { 
            helpers = new Dictionary<Type, IHelper>();
            assemblies = new List<Assembly>();
        }

        public static string ApplicationID = "We7.HelperFactory";

        static bool IsExproe;

        static object _locker = new object();

        public static HelperFactory Instance
        {
            get 
            {
                bool reseted = false;
                bool fromhttpcontext = (null != HttpContext.Current);
                HelperFactory helper;
                if (IsExproe)
                {
                    ResetHelperFactory(fromhttpcontext);
                    reseted = true;
                    IsExproe = false;
                }
                if (reseted || null == (helper = TryAssignHelperFactory(fromhttpcontext)))
                {
                    lock (_locker)
                    {
                        helper = TryAssignHelperFactory(fromhttpcontext);
                        if (null == helper)
                        {
                            CreateFactory(fromhttpcontext);
                            helper = TryAssignHelperFactory(fromhttpcontext);
                        }
                    }
                }
                return helper;
            }
        }

        static void CreateFactory(bool reset)
        {
            object helper = Utils.CreateInstance("We7.CMS.ApplicationHelper,We7.CMS.Install");
            if (helper != null)
            {
                MethodInfo method = helper.GetType().GetMethod(reset ? "ResetApplication" : "LoadHelperFactory");
                if (method != null)
                {
                    method.Invoke(helper, null);
                }
            }
            else
            {
                throw new InvalidOperationException("系统安装状态不确定，无法初始化持久化安装程序");
            }
        }

        static void ResetHelperFactory(bool fromHttpContext)
        {
            lock (_locker)
            {
                if (fromHttpContext)
                    HttpContext.Current.Application[HelperFactory.ApplicationID] = null;
            }
        }

        static HelperFactory TryAssignHelperFactory(bool fromHttpContext)
        {
            return fromHttpContext ? HttpContext.Current.Application[HelperFactory.ApplicationID] as HelperFactory : null;
        }

        public static T GetHelper<T>()
        { 
            Type t = typeof (T);
            if (helpers.ContainsKey(t))
            {
                return (T)helpers[t];
            }
            Load();
            if (helpers.ContainsKey(t))
            {
                return (T)helpers[t];
            }
            throw new TypeLoadException(string.Format("找不到指定类型的 Helper：{0}", t.FullName));
        }

        [field:NonSerialized]
        private event EventHandler<GatheringAssemblyEventArgs> _onGatherAssemblyHandlers;

        private void Load()
        {
            GatheringAssemblyEventArgs args = new GatheringAssemblyEventArgs();
            args.ReferecedAssembies = AppDomain.CurrentDomain.GetAssemblies();

            if (null != _onGatherAssemblyHandlers)
            {
                _onGatherAssemblyHandlers(this, args);
            }
            foreach (Assembly ass in args.ReferecedAssembies)
            {
                ProcessAssembly(ass);
            }
        }

        private void ProcessAssembly(Assembly ass)
        {
            if (!ass.FullName.StartsWith("We7.CMS.Web") && (ass.FullName.StartsWith("We7") || ass.FullName.StartsWith("We7Engine2007")))
            {
                if (!assemblies.Contains(ass))
                {
                    assemblies.Add(ass);
                    try
                    {
                        Type[] types = ass.GetTypes();
                        foreach (Type type in types)
                        {
                            ProcessType(type);
                        }
                    }
                    catch
                    {
                        HttpContext.Current.Response.Write(ass.FullName + "<br />");
                    }
                }
            }
        }

        private void ProcessType(Type type)
        {
            try
            {
                object[] objs = type.GetCustomAttributes(typeof(HelperAttribute), false);
                if (objs != null || objs.Length > 0)
                {
                    foreach (object obj in objs)
                    {
                        HelperAttribute ha = obj as HelperAttribute;
                        if (ha != null)
                        {
                            IHelper helper = Activator.CreateInstance(type) as IHelper;
                            if (helper != null)
                            {
                                helper.Description = ha.Description;
                                helper.Name = ha.Name;
                                helper.Root = Root;
                                helpers.Add(type, helper);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(), ex);
            }
        }

        public class GatheringAssemblyEventArgs : EventArgs
        {
            private ICollection<Assembly> _referencedAssemblies;
            public ICollection<Assembly> ReferecedAssembies
            {
                get { return _referencedAssemblies; }
                internal set { _referencedAssemblies = value; }
            }
        }
    }
}
