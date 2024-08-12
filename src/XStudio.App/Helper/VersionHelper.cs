using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Helper
{
    public class VersionHelper
    {
        public static string GetVersion()
        {
            FileVersionInfo? versionInfo;
            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                versionInfo = null;
            }
            else
            {
                versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            }
#if NET40
        var netVersion = ".NET Framework 4.0";
#elif NET45
        var netVersion = ".NET Framework 4.5";
#elif NET451
        var netVersion = ".NET Framework 4.5.1";
#elif NET452
        var netVersion = ".NET Framework 4.5.2";
#elif NET46
        var netVersion = ".NET Framework 4.6";
#elif NET461
        var netVersion = ".NET Framework 4.6.1";
#elif NET462
        var netVersion = ".NET Framework 4.6.2";
#elif NET47
        var netVersion = ".NET Framework 4.7";
#elif NET471
        var netVersion = ".NET Framework 4.7.1";
#elif NET472
        var netVersion = ".NET Framework 4.7.2";
#elif NET48
        var netVersion = ".NET Framework 4.8";
#elif NET481
        var netVersion = ".NET Framework 4.8.1";
#elif NET5_0
        var netVersion = ".NET 5.0";
#elif NET6_0
        var netVersion = ".NET 6.0";
#elif NET7_0
        var netVersion = ".NET 7.0";
#elif NET8_0
            var netVersion = ".NET 8.0";
#elif NETCOREAPP3_0
        var netVersion = ".NET CORE 3.0";
#elif NETCOREAPP3_1
        var netVersion = ".NET CORE 3.1";
#endif
            if (versionInfo == null)
            {
                return netVersion;
            }
            else
            {
                return $"v{versionInfo.FileVersion} {netVersion}";
            }
        }

        public static string? GetCopyright()
        {
            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                return null;
            }
            else
            {
                return FileVersionInfo.GetVersionInfo(assembly.Location).LegalCopyright;
            }
        }
    }
}
