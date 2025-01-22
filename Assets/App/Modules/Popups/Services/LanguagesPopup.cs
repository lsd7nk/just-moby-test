using Cysharp.Threading.Tasks;
using App.Localization;
using App.Data;

namespace App.Popups
{
    public sealed class LanguagesPopup : Popup<LanguagesPopupView>
    {
        private readonly UserDataService _userData;

        public LanguagesPopup(UserDataService userData)
        {
            _userData = userData;
        }

        public override void Initialize(IPopupView view, params object[] args)
        {
            base.Initialize(view, args);

            _view.SetCurrentLanguage(LocalizationProvider.CurrentLanguage);
            _view.AddTogglesOnClickHandler(OnChangeLanguage);
        }

        private void OnChangeLanguage(LanguageType language)
        {
            if (LocalizationProvider.CurrentLanguage == language)
            {
                return;
            }

            var localeCode = LocalizationProvider.GetLocaleCode(language);
            var locale = LocalizationProvider.GetLocale(localeCode);

            _userData.SetLocaleCode(localeCode);

            LocalizationProvider.Initialize(locale).Forget();
            Hide();
        }
    }
}