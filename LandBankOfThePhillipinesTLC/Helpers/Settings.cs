using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace LandBankOfThePhillipinesTLC.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string LastEmailSettingsKey = "last_email_key";
        private const string LastPasswordSettingsKey = "last_password_key";
        private const string LastScanningnumberSettingsKey = "last_ScanningNumber_key";
        private const string LastFullNameSettingsKey = "last_FullName_key";
        private const string lat1SettingsKey = "last_lat1_key";
        private const string lat2SettingsKey = "last_lat2_key";
        private const string long1SettingsKey = "last_long1_key";
        private const string long2SettingsKey = "last_long2_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static string Lattitude1
        {
            get
            {
                return AppSettings.GetValueOrDefault(lat1SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(lat1SettingsKey, value);
            }
        }
        public static string Lattitude2
        {
            get
            {
                return AppSettings.GetValueOrDefault(lat2SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(lat2SettingsKey, value);
            }
        }
        public static string Longhitude1
        {
            get
            {
                return AppSettings.GetValueOrDefault(long1SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(long1SettingsKey, value);
            }
        }
        public static string Longhitude2
        {
            get
            {
                return AppSettings.GetValueOrDefault(long2SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(long2SettingsKey, value);
            }
        }
        public static string LastUsedEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastEmailSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastEmailSettingsKey, value);
            }
        }
        public static string ScanningNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastScanningnumberSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastScanningnumberSettingsKey, value);
            }
        }
        public static string FullName
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastFullNameSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastFullNameSettingsKey, value);
            }
        }
        public static string LastPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastPasswordSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastPasswordSettingsKey, value);
            }
        }

    }
}
