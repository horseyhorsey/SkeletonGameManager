using System.Diagnostics;

namespace SkeletonGameManager.Base.Helpers
{
    public static class Versions
    {
        public static string GetVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
        }
    }
}
