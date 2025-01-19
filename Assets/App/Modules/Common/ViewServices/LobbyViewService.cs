using App.Common.Views;
using App.Ads;

namespace App.Common
{
    public sealed class LobbyViewService : ViewService<LobbyView>
    {
        private readonly IAdService _adService;

        public LobbyViewService(IAdService adService)
        {
            _adService = adService;
        }

        public override void Initialize()
        {
            _adService.Banner.Allowed = true;
            _adService.Banner.Show();
        }
    }
}
