namespace EnableGPlayWithPC
{
    internal static class Packages
    {
        internal static string Vending = "com.android.vending";
        internal static string GMS = "com.google.android.gms";
        internal static string GSF = "com.google.android.gsf";
        internal static string GSFLogin = "com.google.android.gsf.login";
        internal static string ContactsSyncAdapters = "com.google.android.syncadapters.contacts";
        internal static string CalendarSyncAdapters = "com.google.android.syncadapters.calendar";

        internal static string[] Package = new string[]
        {
            GMS,
            GSFLogin,
            GSF,
            Vending,
            ContactsSyncAdapters,
            CalendarSyncAdapters
        };
    }
}