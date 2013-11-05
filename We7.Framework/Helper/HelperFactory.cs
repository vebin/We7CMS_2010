using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

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
