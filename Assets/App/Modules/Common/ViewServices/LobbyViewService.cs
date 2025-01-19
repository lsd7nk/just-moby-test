using App.Common.Views;
using App.Ads;

namespace App.Common
{
    public sealed class LobbyViewService : ViewService<LobbyView>
    {
        private readonly IAdService _adService;
        private readonly AppConfig _config;

        public LobbyViewService(IAdService adService, AppConfig config)
        {
            _adService = adService;
            _config = config;
        }

        public override void Initialize()
        {
            // to do: add AdInitializationService
            _adService.Banner.Allowed = _config.BannerAdEnabled;
            _adService.Banner.Show();
        }
    }
}
