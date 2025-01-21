using Cysharp.Threading.Tasks;
using App.Common.Views;
using App.AppStates;
using App.Popups;
using App.Ads;

namespace App.Common
{
    public sealed class LobbyViewService : ViewService<LobbyView>
    {
        private readonly IPopupFactoryService _popupFactory;
        private readonly AppStateService _appStates;
        private readonly IAdService _adService;
        private readonly AppConfig _config;

        public LobbyViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig config, AppStateService appStates)
        {
            _popupFactory = popupFactory;
            _appStates = appStates;
            _adService = adService;
            _config = config;
        }

        public override void Initialize()
        {
            // to do: add AdInitializationService
            _adService.Banner.Allowed = _config.BannerAdEnabled;
            _adService.Banner.Show();
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
