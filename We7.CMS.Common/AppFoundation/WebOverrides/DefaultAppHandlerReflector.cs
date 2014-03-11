using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.IO;
using AspNetUI = System.Web.UI;


namespace We7.CMS.Common.AppFoundation
{
    public class DefaultAppHandlerReflector : IAppHandlerReflector
    {
        static PropertyInfo _templatePathProperty;
        static Type _uiInternalPathClass;
        static readonly string[] extensions = new[] { ".aspx",".ashx",".asmx"};
        static readonly string[] staticCodedExtensions = new[] { ".ashx",".asmx"};

        readonly IEnumerable<IAppHost> _hosts;
        readonly IAppDescriptorManager _appDescriptorManager;
        readonly IAppManager _appManager;

        public DefaultAppHandlerReflector(
            IAppDescriptorManager appDescriptorManager,
            IAppManager appManager,
            IEnumerable<IAppHost> hosts
            )
        {
            _appDescriptorManager = appDescriptorManager;
            _appManager = appManager;
            _hosts = hosts;
        }

        public bool IsAppRequest(string requestPath, out IAppHost hostMatched)
        {
            hostMatched = null;
            requestPath = TakeVirtualRequestPath(requestPath);

            if (!IsAspNetRequest(requestPath))
                return false;

            string[] fragments = SplitPath(requestPath);
            if (fragments.Length < 3)
                return false;

            foreach (IAppHost host in _hosts)
            {
                if (host.DirName.Equals(fragments[0], StringComparison.OrdinalIgnoreCase))
                {
                    hostMatched = host;
                    break;
                }
            }
            return null != hostMatched;
        }

        static bool IsAspNetRequest(string requestPath)
        {
            int index = requestPath.LastIndexOf('.');
            string extension = index < 0 ? null : requestPath.Substring(index);
            bool found = false;

            if (null != extension)
            {
                for (int i = 0, l = extension.Length; i < l; i++)
                {
                    if (extensions[i].Equals(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }

        static string TakeVirtualRequestPath(string requestpath)
        {
            requestpath = requestpath.TrimStart('~');
            return AddDefaultDocument(StripApplicationPath(requestpath));
        }

        static string[] SplitPath(string requestPah)
        {
            return requestPah.Split(new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries);
        }

        static string StripApplicationPath(string requestpath)
        {
            string appPath = HttpRuntime.AppDomainAppVirtualPath;
            return appPath.Length == 1
                ? requestpath : requestpath.Substring(appPath.Length);
        }

        static string AddDefaultDocument(string path)
        {
            if (path.EndsWith("/"))
            {
                path += "Default.aspx";
            }
            return path;
        }

        static bool IsStaticCodedHandler(string lastFragment)
        {
            foreach (string extension in staticCodedExtensions)
            {
                if (lastFragment.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        static string StaticCodedHandlerType(string appId, string[] fragments)
        {
            StringBuilder typeNameBuilder = new StringBuilder(appId);
            int length = fragments.Length;
            for (int i = 0; i < length; i++)
            {
                typeNameBuilder.Append(".");
                if (i == length - 1)
                {
                    fragments[i] = fragments[i].Substring(0, fragments[i].LastIndexOf('.'));
                }
                typeNameBuilder.Append(fragments[i]);
            }

            return typeNameBuilder.ToString();
        }

        static string HandlerTypeName(string[] fragments)
        {
            StringBuilder typeNameBuilder = new StringBuilder();
            int length = fragments.Length;
            for (int i = 0; i < length; i++)
            {
                typeNameBuilder.Append(".");
                typeNameBuilder.Append(fragments[i].ToLowerInvariant());
            }
            typeNameBuilder.Remove(0, 1);
            return typeNameBuilder.Replace('.', '_').Insert(0, "ASP.").ToString();
        }

        public IHttpHandler GetHandlerFromApp(string requestPath)
        {
            requestPath = TakeVirtualRequestPath(requestPath);
            Type typeFound = GetHandlerTypeFromApp(SplitPath(requestPath));

            if (null != typeFound)
            {
                IHttpHandler handler = null;
                try
                {
                    handler = Activator.CreateInstance(typeFound) as IHttpHandler;
                }
                catch (Exception ex)
                {
                    handler = null;
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(string.Format("无法创建页面 '{0}' 的实例，发生的异常为 {1}", typeFound.FullName, ex.Message));
#endif
                }
                if (null == handler)
                    return null;
                AspNetUI.Page page = handler as AspNetUI.Page;
                Type uiPageType = typeof(AspNetUI.Page);

                if (null == _templatePathProperty)
                {
                    _templatePathProperty = uiPageType.GetProperty("TemplateControlVirtualPath",
                                                                    BindingFlags.NonPublic | BindingFlags.Instance);
                    _uiInternalPathClass = uiPageType.Assembly.GetType("System.Web.VirtualPath");
                }

                if (null != page)
                {
                    string relativePath = "~" + requestPath;
                    page.AppRelativeVirtualPath = relativePath;
                    Type pageType = page.GetType();
                    FieldInfo dependencyField = pageType.GetField("__fileDependencies", BindingFlags.NonPublic | BindingFlags.Static);
                    if (null != dependencyField)
                    {
                        MethodInfo getDependencyMethod = uiPageType.GetMethod("GetWrappedFileDependencies", 
                                                                                BindingFlags.NonPublic | BindingFlags.Instance);
                        object wrappedDependency = getDependencyMethod.Invoke(page, new object[]{new string[]{relativePath}});
                        dependencyField.SetValue(null, wrappedDependency);
                    }

                    MethodInfo method = _uiInternalPathClass.GetMethod("Create", new Type[]{typeof(string)});
                    object virtualPath = method.Invoke(null, new object[] {requestPath});
                    _templatePathProperty.SetValue(page, virtualPath, null);
                }
                return handler;
            }
            return null;
        }

        Type GetHandlerTypeFromApp(string[] pathFragments)
        {
            string appId = pathFragments[1];
            AppInfo appInfo = _appManager.GetAppInfo(appId);

            if (null == appInfo || !appInfo.IsEnable)
                return null;

            int length = pathFragments.Length;
            string[] striped = new string[length - 2];
            for (int i=2; i<length; i++)
            {
                striped[i-2] = pathFragments[i];
            }

            string typeName = IsStaticCodedHandler(striped[striped.Length - 1])
                                    ? StaticCodedHandlerType(appId, striped)
                                    : HandlerTypeName(striped);
            return GetTypeFromApp(appInfo, typeName);
        }

        Type GetTypeFromApp(AppInfo appInfo, string typeName)
        {
            Type typeFound = null;
            IEnumerable<Assembly> assemblies = _appDescriptorManager.GetAppAssembly(appInfo);

            foreach (Assembly assembly in assemblies)
            {
                if (null != assembly)
                {
                    try
                    {
                        typeFound = assembly.GetType(typeName);
                    }
                    catch (Exception)
                    { 
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(string.Format("加载类型出错 '{0}'，无法从程序集 {1} 中加载该类型。"),typeName, assembly.FullName);
#endif
                    }
                    if (null != typeFound)
                        break;
                }
            }

            return typeFound;
        }
    }
}
