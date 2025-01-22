using UnityEngine;

namespace App.Data
{
    public sealed class UserDataService
    {
        public bool VibrationEnabled { get; private set; }
        public string LocaleCode { get; private set; }

        public UserDataService()
        {
            Initialize();
        }

        public void SetLocaleCode(string localeCode)
        {
            LocaleCode = localeCode;
        }

        public void SetVibrationEnabled(bool state)
        {
            VibrationEnabled = state;
        }

        public void Save()
        {
            PlayerPrefs.SetInt(Prefs.VIBRATION_KEY, VibrationEnabled ? 1 : 0);
            PlayerPrefs.SetString(Prefs.LOCALE_KEY, LocaleCode);
        }

        private void Initialize()
        {
            LocaleCode = PlayerPrefs.GetString(Prefs.LOCALE_KEY);
            VibrationEnabled = PlayerPrefs.GetInt(Prefs.VIBRATION_KEY, 1) == 1
                ? true
                : false;
        }
    }
}