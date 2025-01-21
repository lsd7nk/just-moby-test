using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using Cysharp.Threading.Tasks;
using App.Localization;
using UnityEngine;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class LocalizationLoadingSegment : LoadingSegment
    {
        public override async UniTask Init()
        {
            await LocalizationProvider.Initialize(GetLocale());
        }

        private Locale GetLocale()
        {
            string localeCode = PlayerPrefs.GetString("l"); // to do

            if (!string.IsNullOrEmpty(localeCode))
            {
                int availableLocalesCount = LocalizationSettings.AvailableLocales.Locales.Count;

                for (int i = 0; i < availableLocalesCount; ++i)
                {
                    var availableLocale = LocalizationSettings.AvailableLocales.Locales[i];

                    if (availableLocale.Identifier.Code == localeCode)
                    {
                        return availableLocale;
                    }
                }
            }

            return LocalizationSettings.ProjectLocale;
        }
    }
}
