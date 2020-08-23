using System;
using SQLite;

namespace LandBankOfThePhillipinesTLC.Constants
{
    public static class AppConstants
    {
        //public const string SyncfusionLicenseKey = "MjY1ODM5QDMxMzgyZTMxMmUzMFEzNENoTi9TY1pGK2FjL1NkOTRqWjEveDRDL1A0NXpjQWQ3V0RvUWY5Skk9";
        //public const string Key = "4vmdEbakP7CLEAwB";
        //public const int OTPDeviceID = 115601;
        public const string DatabaseFileName = "lbptlc.db3";
        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache;
    }
}
