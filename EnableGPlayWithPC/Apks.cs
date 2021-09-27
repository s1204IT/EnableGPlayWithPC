namespace EnableGPlayWithPC
{
    internal static class Apks
    {
        internal static string Vending = "apk\\GooglePlayStore.apk";
        internal static string GMS = "apk\\GooglePlayServices.apk";
        internal static string GSF = "apk\\GoogleServicesFramework.apk";
        internal static string GSFLogin = "apk\\GoogleLoginService.apk";
        internal static string ContactsSyncAdapters = "bin\\apk\\common\\GoogleContactsSyncAdapter.apk";
        internal static string CalendarSyncAdapters = "bin\\apk\\common\\GoogleCalendarSyncAdapter.apk";
        internal static string GooglePartnerSetup = "bin\\apk\\common\\GooglePartnerSetup.apk";

        internal static string[] installList = new string[]
        {
            ContactsSyncAdapters,
            CalendarSyncAdapters,
            GooglePartnerSetup
        };
    }
}
