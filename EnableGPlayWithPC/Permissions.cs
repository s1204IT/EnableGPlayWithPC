namespace EnableGPlayWithPC {
    internal static class Permissions {
        internal static string Prefix = "android.permission.";

        internal static string[] GSFLogin = new string[]
        {
            "DUMP",
            "READ_LOGS"
        };

        internal static string[] GSF = new string[]
        {
            "DUMP",
            "READ_LOGS",
            "WRITE_SECURE_SETTINGS",
            "INTERACT_ACROSS_USERS"
        };

        internal static string[] GMS = new string[]
        {
            "INTERACT_ACROSS_USERS",
            "PACKAGE_USAGE_STATS",
            "GET_APP_OPS_STATS",
            "READ_LOGS"
        };

        internal static string[] Vending = new string[]
        {
            "PACKAGE_USAGE_STATS",
            "BATTERY_STATS",
            "DUMP",
            "GET_APP_OPS_STATS",
            "INTERACT_ACROSS_USERS",
            "WRITE_SECURE_SETTINGS"
        };
    }
}
