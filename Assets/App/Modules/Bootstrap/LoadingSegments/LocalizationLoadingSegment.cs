using Cysharp.Threading.Tasks;
using App.Localization;
using UnityEngine;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class LocalizationLoadingSegment : LoadingSegment
    {
        public override async UniTask Init()
        {
            string localeCode = PlayerPrefs.GetString("l"); // to do
            var locale = LocalizationProvider.GetLocale(localeCode);

            await LocalizationProvider.Initialize(locale);
        }
    }
}
