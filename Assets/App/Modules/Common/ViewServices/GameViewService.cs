using App.Common.Views;
using App.Popups;
using App.Ads;

namespace App.Common
{
    public sealed class GameViewService : ViewService<GameView>
    {
        private readonly IPopupFactoryService _popupFactory;

        public GameViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig config, AppStateService appStates)
        {
            _popupFactory = popupFactory;
        }

        protected override void OnViewSet()
        {
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
