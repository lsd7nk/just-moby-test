using Cysharp.Threading.Tasks;
using App.Localization;
using App.AppStates;
using UnityEngine;
using App.Common;
using App.Events;
using System;

namespace App.Popups
{
    public sealed class SettingsPopup : Popup<SettingsPopupView>, IDisposable
    {
        private const string LANGUAGE_KEY_TEMPLATE = "language/{0}";
        private const string VERSION_KEY = "version";

        private readonly IPopupFactoryService _popupFactory;
        private readonly AppStateService _appStates;

        private bool _inGameState;

        public SettingsPopup(AppStateService appStates, IPopupFactoryService popupFactory)
        {
            _popupFactory = popupFactory;
            _appStates = appStates;
        }

        public override void Initialize(IPopupView view, params object[] args)
        {
            base.Initialize(view, args);

            if (args.Length > 0)
            {
                _inGameState = (bool)args[0];
            }

            OnLocalize();

            _view.RefreshButtons(_inGameState);
            _view.AddLanguageButtonOnCickHandler(OnLanguageButtonClick);

            if (_inGameState)
            {
                _view.AddRestartButtonOnCickHandler(OnRestartButtonClick);
                _view.AddLobbyButtonOnCickHandler(OnLobbyButtonClick);
            }

            LocalizationProvider.OnLocalizeEvent += OnLocalize;
        }

        public void Dispose()
        {
            LocalizationProvider.OnLocalizeEvent -= OnLocalize;
        }

        private void OnLocalize()
        {
            string localeCode = LocalizationProvider.GetCurrentLocaleCode();
            string key = string.Format(LANGUAGE_KEY_TEMPLATE, localeCode);

            string versionText = LocalizationProvider.GetText(VERSION_KEY);
            string languageText = LocalizationProvider.GetText(key);

            _view.SetLanguageText(languageText);
            _view.SetVersionText(string.Format(versionText, Application.version));
        }

        private void OnLanguageButtonClick()
        {
            _popupFactory.ShowPopup<LanguagesPopup>();
        }

        private void OnRestartButtonClick()
        {
            _view.Hide();
            EventSystem.Send<RestartLevelEvent>();
        }

        private void OnLobbyButtonClick()
        {
            _view.Hide();
            _appStates.GoToState(AppStateType.Lobby, new AppStatePayload()).Forget();
        }
    }
}
