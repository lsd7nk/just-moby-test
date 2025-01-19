using UnityEngine;
using App.Events;
using App.Ads;

namespace App.UI
{
    public abstract class BannerAdjuster<TTransform> : BannerAdjuster where TTransform : Transform
    {
        [SerializeField] protected TTransform[] adjustableViews;
    }

    public abstract class BannerAdjuster : MonoBehaviour
    {
        protected IAdService _adService;

        protected bool IsBannerShowing => _adService?.Banner?.IsShowing ?? false;
        protected bool IsBannerAllowing => _adService?.Banner?.Allowed ?? false;

        public void Initialize(IAdService adService)
        {
            _adService = adService;
            Initialize();
        }

        protected void Initialize()
        {
            if (IsBannerShowing)
            {
                OnBannerShown();
            }
            else
            {
                OnBannerHidden();
            }
        }

        protected void OnBannerShown(BannerShownEvent e)
        {
            OnBannerShown();
        }

        protected void OnBannerHidden(BannerHiddenEvent e)
        {
            OnBannerHidden();
        }

        protected abstract void OnBannerShown();
        protected abstract void OnBannerHidden();

        protected void OnEnable()
        {
            EventSystem.Subscribe<BannerShownEvent>(OnBannerShown);
            EventSystem.Subscribe<BannerHiddenEvent>(OnBannerHidden);
        }

        protected void OnDisable()
        {
            EventSystem.Unsubscribe<BannerShownEvent>(OnBannerShown);
            EventSystem.Unsubscribe<BannerHiddenEvent>(OnBannerHidden);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Initialize();
            }
        }
    }
}
