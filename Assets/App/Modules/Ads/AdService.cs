using MiniContainer;

namespace App.Ads
{
    public interface IAdService
    {
        IBannerAd Banner { get; }

        void Initialize();
        void SetUserId(string userId);
    }


    public sealed class AdService : IronSourceAdService, IContainerApplicationPauseListener
    {
#if UNITY_ANDROID
		private const string APP_KEY = "20c643a55";
#else
        private const string APP_KEY = "";
#endif

        public AdService() : base(APP_KEY, true)
        {
            Initialize();
        }
    }
}