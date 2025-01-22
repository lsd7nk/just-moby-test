using Cysharp.Threading.Tasks;
using App.Localization;
using App.Data;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class LocalizationLoadingSegment : LoadingSegment
    {
        private readonly UserDataService _userData;

        public LocalizationLoadingSegment(UserDataService userData)
        {
            _userData = userData;
        }

        public override async UniTask Init()
        {
            string localeCode = _userData.LocaleCode;
            var locale = LocalizationProvider.GetLocale(localeCode);

            await LocalizationProvider.Initialize(locale);
        }
    }
}
