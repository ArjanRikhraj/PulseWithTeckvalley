// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Pulse
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
        public static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }
        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AccessTokenKey, value);
            }
        }
        public static bool IsOnBoardingShown
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(OnBoardingKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(OnBoardingKey, value);
            }
        }

        #region Setting Constants
        public const string OnBoardingKey = "onBoarding_key";
        const string AccessTokenKey = "access_token";
        static string AccessTokenDefault = string.Empty;
        #endregion


  }
}