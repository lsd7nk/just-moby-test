#if UNITY_EDITOR
using System.Threading.Tasks;
using UnityEngine;

namespace App.Ads
{
    public abstract class DummyBannerAd : AbstractBaseBannerAd
    {
        private bool _loaded = false;
        private GameObject _gameObject;

        protected override void InvokeInternalInit()
        {
            InvokeLoad();
        }

        protected override void InvokeInternalShow()
        {
            if (_loaded)
            {
                DisplayBanner();
            }
            else
            {
                InvokeLoad();
            }
        }

        protected override void InvokeInternalHide()
        {
            if (_gameObject != null)
            {
                Object.Destroy(_gameObject);
                _gameObject = null;
            }
        }

        protected override async void InvokeLoad()
        {
            if (_loaded)
                return;

            await Task.Delay(3000);
            _loaded = true;
            OnBannerAdLoaded();
        }

        private void OnBannerAdLoaded()
        {
            _loaded = true;

            if (_isShowing)
            {
                DisplayBanner();
            }
        }

        private void DisplayBanner()
        {
            if (_gameObject != null)
                return;

            _gameObject = new GameObject("-DummyBanner-");
            _gameObject.AddComponent<DummyBannerMonoBehaviour>();
        }
    }
}
#endif
