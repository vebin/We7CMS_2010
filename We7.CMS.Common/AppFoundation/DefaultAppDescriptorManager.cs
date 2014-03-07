using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using We7.CMS.Common.AppFoundation.Services;
using We7.Framework.FileSystem;
using We7.Framework;
using System.IO;
using System.Web.Hosting;
using System.Security.Permissions;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;


namespace We7.CMS.Common.AppFoundation
{
    public class DefaultAppDescriptorManager : IAppDescriptorManager
    {
        readonly Dictionary<AppInfo, IEnumerable<Assembly>> _appInfoDic;
        readonly Dictionary<string, Assembly> _appReferencedAssemblies;
        readonly Dictionary<string, AppInfo> _memoryAppInfo;
        readonly object _resolveAssemblylocker = new object();

        readonly IEnumerable<IAppHost> _appHosts;
        readonly IAppStateManager _appStateManager;
        readonly IVirtualPathProvider _virtualPathProvider;
        readonly Regex _schemeRegex = new Regex(@"^([a-z]+)\:\/\/.+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        static bool _assemblyResolveEventAdded = false;

        public DefaultAppDescriptorManager(IEnumerable<IAppHost> appHosts,
            IAppStateManager appStateManager,
            IVirtualPathProvider virtualPathProvider)
        {
            _appHosts = appHosts;
            _appStateManager = appStateManager;
            _virtualPathProvider = virtualPathProvider;

            _appInfoDic = new Dictionary<AppInfo, IEnumerable<Assembly>>();
            _appReferencedAssemblies = new Dictionary<string, Assembly>();
            _memoryAppInfo = new Dictionary<string, AppInfo>();
            InitAppInfo();
        }

        public IEnumerable<AppInfo> GetApps()
        {
            return _appInfoDic.Keys;
        }

        public IEnumerable<AppInfo> GetApps(IAppHost host)
        {
            string hostDirPath = _virtualPathProvider.MapPath(string.Format("~/{0}/", host.DirName));
            DirectoryInfo directory = new DirectoryInfo(hostDirPath);
            if (!directory.Exists)
                return Enumerable.Empty<AppInfo>();
            Stack<AppInfo> aSet = new Stack<AppInfo>();
            IEnumerable<string> enabledapps = _appStateManager.EnabledAppIds();

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                DirectoryInfo[] appbin = dir.GetDirectories("bin", SearchOption.TopDirectoryOnly);
                if (appbin.Length != 1)
                    continue;

                string basicAssemblyPath = Path.Combine(Path.Combine(Path.Combine(hostDirPath, dir.Name), "bin"), dir.Name + ".dll");
                if (_memoryAppInfo.ContainsKey(basicAssemblyPath))
                {
                    aSet.Push(_memoryAppInfo[basicAssemblyPath]);
                }
                else
                {
                    Assembly assembly, resourcesAssembly;
                    AppInfo app = LoadApp(host, basicAssemblyPath, out assembly, out resourcesAssembly);

                    if (app != null)
                    {
                        app.IsEnable = false;
                        foreach (string item in enabledapps)
                        {
                            if (string.Equals(item, app.Id))
                            {
                                app.IsEnable = true;
                                break;
                            }
                        }

                        if (!_appInfoDic.ContainsKey(app))
                        {
                            Assembly[] assemblies = new Assembly[]{
                                                    assembly,
                                                    app.IsEnable ? resourcesAssembly : null
                            };
                            _appInfoDic.Add(app, assemblies);
                        }
                        _memoryAppInfo.Add(basicAssemblyPath, app);
                        aSet.Push(app);
                    }
                }
            }
            return aSet;
        }

        public IEnumerable<Assembly> GetAppAssembly(AppInfo appInfo)
        {
            return (null == appInfo || _appInfoDic.ContainsKey(appInfo))
                ? Enumerable.Empty<Assembly>() : _appInfoDic[appInfo];
        }

        void InitAppInfo()
        {
            foreach (IAppHost appHost in _appHosts)
            {
                GetApps(appHost);
            }
        }

        AppInfo LoadApp(IAppHost host, string fullName, out Assembly includedAssembly, out Assembly resourcesAssembly)
        {
            resourcesAssembly = null;
            includedAssembly = LoadAssembly(fullName);

            Type appBaseType = typeof(AppMetadata);
            Type[] appTypes = includedAssembly.GetTypes();
            Type appType = null;

            foreach (var type in appTypes)
            {
                if (appBaseType.IsAssignableFrom(type))
                {
                    appType = type;
                    break;
                }
            }
            if (appType == null)
                return null;
            string resourcesAssemblyName = fullName.Substring(0, fullName.Length - 3) + "Resources.dll";
            if (File.Exists(resourcesAssemblyName))
            {
                resourcesAssembly = LoadAssembly(resourcesAssemblyName);
            }
            AppMetadata metadata = Activator.CreateInstance(appType) as AppMetadata;
            PreProcessAppMetadata(ref metadata, host);

            return new AppInfo(metadata);
        }

        void PreProcessAppMetadata(ref AppMetadata metadata, IAppHost currentHost)
        {
            if (null != metadata.MenuItems)
            {
                foreach (AppMenuItem menu in metadata.MenuItems)
                {
                    menu.Url = ProcessAppMenuUrl(menu.Url, metadata.Id, currentHost);
                }
            }

            string[] fragments = new string[]{
                                    string.Empty,
                                    currentHost.DirName,
                                    metadata.Id,
                                    "App.png"
                                };
            metadata.Logo = string.Join("/", fragments);
        }

        string ProcessAppMenuUrl(string url, string appId, IAppHost host)
        {
            if (null == url)
                url = string.Empty;
            if (url.Length > 0)
            {
                if (IsAbsoluteMenu(url) || url[0].Equals('/'))
                {
                    return url;
                }
            }

            string appPath = HttpRuntime.AppDomainAppVirtualPath;
            if (appPath.Length == 1)
                appPath = string.Empty;
            if (url.StartsWith("~/"))
                return string.Concat(appPath, "/", url.Substring(2));

            string[] fragments = new string[] { appPath, host.DirName, appId, string.Empty};

            return string.Concat(string.Join("/", fragments), url);
        }

        bool IsAbsoluteMenu(string url)
        {
            return _schemeRegex.IsMatch(url);
        }


        Assembly LoadAssembly(string fullname)
        {
            if (string.IsNullOrEmpty(fullname) || fullname.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("只能加载.dll程序", "fullname");
            if (!_assemblyResolveEventAdded)
            {
                _assemblyResolveEventAdded = true;
                AppDomain.CurrentDomain.AssemblyResolve += currentDomain_AssemblyResolve;
            }

            Assembly includedAssembly = null;
            try
            {
                new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, fullname).Assert();
                FileStream fs = File.Open(fullname, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                includedAssembly = Assembly.Load(bytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("无法加载的程序集。", ex);
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
            return includedAssembly;
        }

        Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            string fullname = e.Name;
            string name = fullname.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];

            if (IsResolvingSerializers(name))
            {
                return null;
            }
            Assembly result;
            if (ResolveAppAssemblyForDomain(name, fullname, out result))
            {
                return result;
            }

            foreach (IAppHost host in _appHosts)
            {
                result = ResolveAssemblyFromHost(host, fullname);
                if (result != null)
                    return result;
            }

            return null;
        }

        static bool IsResolvingSerializers(string name)
        {
            return name.StartsWith("We7") && name.EndsWith("XmlSerializers");
        }

        bool ResolveAppAssemblyForDomain(string name, string fullname, out Assembly result)
        {
            result = null;
            if (name.EndsWith(".Resources.dll"))
                name = name.Split(new char[]{'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
            foreach (IAppHost host in _appHosts)
            {
                string hostPath = _virtualPathProvider.MapPath(string.Format("~/{0}/", host.DirName));
                string basicDllPath = Path.Combine(Path.Combine(Path.Combine(hostPath, name), "bin"), name+".dll");

                AppInfo app;
                if (_memoryAppInfo.TryGetValue(basicDllPath, out app))
                {
                    IEnumerable<Assembly> appAssemblies = _appInfoDic[app];
                    foreach (Assembly assembly in appAssemblies)
                    {
                        if (assembly.FullName == fullname)
                        {
                            result = assembly;
                            break;
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        Assembly ResolveAssemblyFromHost(IAppHost host, string fullname)
        {
            Assembly assembly;
            bool gotValue;
            lock (_resolveAssemblylocker)
            {
                gotValue = _appReferencedAssemblies.TryGetValue(fullname, out assembly);
            }

            if (gotValue)
            {
                return assembly;
            }

            string hostDirPath = HostingEnvironment.MapPath(string.Format("~/{0}/", host.DirName));
            DirectoryInfo directory = new DirectoryInfo(hostDirPath);
            if (!directory.Exists)
                return null;
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                string appName = dir.Name;
                AppInfo app;
                if (_memoryAppInfo.TryGetValue(appName, out app)
                    && app.IsEnable)
                {
                    string basicAssemblyPath = Path.Combine(Path.Combine(host.DirName, dir.Name), "bin");
                    DirectoryInfo appBinDir = new DirectoryInfo(basicAssemblyPath);
                    if (appBinDir.Exists)
                    {
                        string appResourceName = appName + ".Resources.dll";
                        FileInfo[] dllFiles = appBinDir.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
                        foreach (FileInfo dllFile in dllFiles)
                        {
                            if (dllFile.Name.Equals(appName, StringComparison.OrdinalIgnoreCase)
                                && dllFile.Name.Equals(appResourceName, StringComparison.OrdinalIgnoreCase))
                            {
                                Assembly loadedAssembly = null;
                                try
                                {
                                    loadedAssembly = LoadAssembly(dllFile.FullName);
                                }
                                catch (Exception ex)
                                { }

                                if (loadedAssembly != null && loadedAssembly.FullName == fullname)
                                {
                                    lock (_resolveAssemblylocker)
                                    {
                                        if (!_appReferencedAssemblies.ContainsKey(fullname))
                                        {
                                            _appReferencedAssemblies.Add(fullname, loadedAssembly);
                                        }
                                    }
                                    return loadedAssembly;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
