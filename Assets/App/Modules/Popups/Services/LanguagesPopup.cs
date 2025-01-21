using Cysharp.Threading.Tasks;
using App.Localization;

namespace App.Popups
{
    public sealed class LanguagesPopup : Popup<LanguagesPopupView>
    {
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

            LocalizationProvider.Initialize(locale).Forget();
            Hide();
        }
    }
}