using Cysharp.Threading.Tasks;
using App.AppStates;
using App.Common;

namespace App.Popups
{
    public sealed class SettingsPopup : Popup<SettingsPopupView>
    {
        private readonly AppStateService _appStates;
        private bool _inGameState;

        public SettingsPopup(AppStateService appStates)
        {
            _appStates = appStates;
        }

        public override void Initialize(IPopupView view, params object[] args)
        {
            base.Initialize(view, args);

            if (args.Length > 0)
            {
                _inGameState = (bool)args[0];
            }

            _view.RefreshButtons(_inGameState);

            if (_inGameState)
            {
                _view.AddRestartButtonOnCickHandler(OnRestartButtonClick);
                _view.AddLobbyButtonOnCickHandler(OnLobbyButtonClick);
            }
        }

        private void OnRestartButtonClick()
        {
            // to do
        }

        private void OnLobbyButtonClick()
        {
            _view.HideAsync().ContinueWith(() =>
            {
                _appStates.GoToState(AppStateType.Lobby, new AppStatePayload()).Forget();
            });
        }
    }
}
