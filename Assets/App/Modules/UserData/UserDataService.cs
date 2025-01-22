using UnityEngine;
using System;

namespace App.Data
{
    public sealed class UserDataService : IDisposable
    {
        public bool VibrationEnabled { get; private set; }
        public string LocaleCode { get; private set; }

        private readonly AppConfig _config;

        public UserDataService(AppConfig config)
        {
            _config = config;
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

        public void Dispose()
        {
            Save();
        }

        private void Initialize()
        {
            LocaleCode = PlayerPrefs.GetString(Prefs.LOCALE_KEY);

            int defaultVibrationEnabled = _config.VibrationEnabled
                ? 1 : 0;

            VibrationEnabled = PlayerPrefs.GetInt(Prefs.VIBRATION_KEY, defaultVibrationEnabled) == 1
                ? true
                : false;
        }

        private void Save()
        {
            PlayerPrefs.SetInt(Prefs.VIBRATION_KEY, VibrationEnabled ? 1 : 0);
            PlayerPrefs.SetString(Prefs.LOCALE_KEY, LocaleCode);
        }
    }
}