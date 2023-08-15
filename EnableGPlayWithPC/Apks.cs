using System.IO;

namespace EnableGPlayWithPC {
    internal static class Apks {
        internal static string Vending = "apps\\Phonesky.apk";
        internal static string old_Vending = "apps\\old\\Phonesky.apk";
        internal static string GMS = "apps\\GmsCore.apk";
        internal static string old_GMS = "apps\\old\\GmsCore.apk";
        internal static string GSF = "apps\\GoogleServicesFramework.apk";
        internal static string GSFLogin = "apps\\GoogleLoginService.apk";

        internal static string[] GAppsInstallList(string appDir) {
            string[] files = { Path.Combine(appDir, GSFLogin), Path.Combine(appDir, GSF), Path.Combine(appDir, GMS), Path.Combine(appDir, Vending) };
            return files;
        }

        internal static string[] Old_GAppsInstallList(string appDir) {
            string[] files = { Path.Combine(appDir, GSFLogin), Path.Combine(appDir, GSF), Path.Combine(appDir, old_GMS), Path.Combine(appDir, old_Vending) };
            return files;
        }
    }
}