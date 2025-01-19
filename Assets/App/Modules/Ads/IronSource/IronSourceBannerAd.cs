using System.Threading.Tasks;

namespace App.Ads
{
    public class IronSourceBannerAd : BannerAd
    {
        private bool _loaded = false;

        protected override void InternalInit()
        {
            IronSourceBannerEvents.onAdLoadedEvent += OnBannerAdLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent += OnBannerAdLoadFailed;

            Load();
        }

        protected override void InternalShow()
        {
            if (_loaded)
            {
                IronSource.Agent.displayBanner();
            }
            else
            {
                Load();
            }
        }

        protected override void InternalHide()
        {
            IronSource.Agent.hideBanner();
        }

        protected override void Load()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        }

        private void OnBannerAdLoaded(IronSourceAdInfo adInfo)
        {
            _loaded = true;

            if (_isShowing)
            {
                IronSource.Agent.displayBanner();
            }
        }

        private void OnBannerAdLoadFailed(IronSourceError error)
        {
            _loaded = false;
            DelayedLoad();
        }

        private async void DelayedLoad()
        {
            await Task.Delay(5000);

            if (!_loaded)
            {
                Load();
            }
        }
    }
}
