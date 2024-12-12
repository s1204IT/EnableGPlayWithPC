using System.IO;

namespace EnableGPlayWithPC {
    internal static class Apks {
        internal static string GmsCore = "apk\\gsa\\GmsCore.apk";
        internal static string Phonesky = "apk\\gsa\\Phonesky.apk";
        internal static string SetupWarning = "apk\\tool\\SetupWarning.apk";

        internal static string[] GAppsInstallList_2(string appDir) {
            string[] files = {
                Path.Combine(appDir, GmsCore),
                Path.Combine(appDir, Phonesky)
            };
            return files;
        }

        internal static string[] ToolAppsInstallList(string appDir) {
            string[] files = {
                Path.Combine(appDir, SetupWarning)
            };
            return files;
        }
    }
}
