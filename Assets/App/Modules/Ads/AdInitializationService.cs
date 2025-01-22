namespace App.Ads
{
    public sealed class AdInitializationService
    {
        private readonly IAdService _adService;
        private readonly AppConfig _config;

        public AdInitializationService(AppConfig config, IAdService adService)
        {
            _adService = adService;
            _config = config;
        }

        public void Initialize()
        {
            var bannerAllowed = _config.BannerAdEnabled;

            _adService.Banner.Allowed = bannerAllowed;

            if (bannerAllowed)
            {
                _adService.Banner.Show();
            }
            else
            {
                _adService.Banner.Hide();
            }
        }
    }
}