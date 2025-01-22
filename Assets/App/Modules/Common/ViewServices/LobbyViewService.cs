using Cysharp.Threading.Tasks;
using App.Common.Views;
using App.AppStates;
using App.Popups;
using App.Ads;

namespace App.Common
{
    public sealed class LobbyViewService : ViewService<LobbyView>
    {
        private readonly AdInitializationService _adInitialization;
        private readonly IPopupFactoryService _popupFactory;
        private readonly AppStateService _appStates;

        public LobbyViewService(IPopupFactoryService popupFactory,
            AdInitializationService adInitialization, AppStateService appStates)
        {
            _adInitialization = adInitialization;
            _popupFactory = popupFactory;
            _appStates = appStates;
        }

        public override void Initialize()
        {
            _adInitialization.Initialize();
        }

        protected override void OnViewSet()
        {
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
            _view.AddPlayButtonOnCickHandler(OnPlayButtonClick);
        }

        private void OnPlayButtonClick()
        {
            _appStates.GoToState(AppStateType.Game, new AppStatePayload()).Forget();
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(false);
        }
    }
}
