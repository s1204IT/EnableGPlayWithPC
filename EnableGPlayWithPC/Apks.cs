using System.IO;

namespace EnableGPlayWithPC {
    internal static class Apks {
        internal static string Vending = "apk\\GooglePlayStore.apk";
        internal static string GMS = "apk\\GooglePlayServices.apk";
        internal static string NEO_GMS = "apk\\3-or-later\\GooglePlayServices.apk";
        internal static string GSF = "apk\\GoogleServicesFramework.apk";
        internal static string GSFLogin = "apk\\GoogleLoginService.apk";
        internal static string ContactsSyncAdapters = "bin\\apk\\common\\GoogleContactsSyncAdapter.apk";
        internal static string CalendarSyncAdapters = "bin\\apk\\common\\GoogleCalendarSyncAdapter.apk";

        internal static string[] GAppsInstallList(string appDir) {
            string[] files = { Path.Combine(appDir, GSF), Path.Combine(appDir, GSFLogin), Path.Combine(appDir, GMS), Path.Combine(appDir, Vending) };
            return files;
        }

        internal static string[] NEO_GAppsInstallList(string appDir) {
            string[] files = { Path.Combine(appDir, GSF), Path.Combine(appDir, GSFLogin), Path.Combine(appDir, NEO_GMS), Path.Combine(appDir, Vending) };
            return files;
        }

        internal static string[] GAppsOtherInstallList = new string[] {
            CalendarSyncAdapters,
            ContactsSyncAdapters
        };
    }
}