using System.Reflection;

namespace Soario.Utilities;

public static class Utils
{
    public static string GetAsmVer(Assembly assembly)
    {
        var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        if (assemblyVersionAttribute is null)
        {
            return assembly.GetName().Version?.ToString() ?? "";
        }
        else
        {
            return assemblyVersionAttribute.InformationalVersion;
        }
    }
}
