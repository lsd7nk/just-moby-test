using System.Collections.Generic;
using UnityEngine;

namespace App.Ads
{
    public struct IronSourceAdServiceConfig
    {
        public readonly string AppKey;
        public readonly bool BannerEnabled;

        public IronSourceAdServiceConfig(string appKey, bool bannerEnabled)
        {
            AppKey = appKey;
            BannerEnabled = bannerEnabled;
        }
    }

    public class IronSourceAdService : IAdService
    {
        public IBannerAd Banner { get; }

        private bool _initializationStarted;
        private readonly IronSourceAdServiceConfig _config;

        public IronSourceAdService(string appKey, bool bannerEnabled)
            : this(new IronSourceAdServiceConfig(appKey, bannerEnabled)) { }

        public IronSourceAdService(IronSourceAdServiceConfig config)
        {
            _config = config;

            if (config.BannerEnabled)
            {
                Banner = new IronSourceBannerAd();
            }
            else
            {
                Banner = new NullBannerAd();
            }
        }

        public void SetUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            IronSource.Agent.setUserId(userId);
        }

        public void ShowDebugView()
        {
            IronSource.Agent.launchTestSuite();
        }

        public virtual void OnApplicationPause(bool pause)
        {
            IronSource.Agent.onApplicationPause(pause);
        }

        public void Initialize()
        {
            if (_initializationStarted)
            {
                return;
            }

            _initializationStarted = true;

            InitIronSource(_config);
        }

        private void InitIronSource(IronSourceAdServiceConfig config)
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializationCompleted;
            IronSourceEvents.onImpressionDataReadyEvent += OnImpressionDataReady;

            IronSource.Agent.shouldTrackNetworkState(true);
            IronSource.Agent.setConsent(true); // edit this
            IronSource.Agent.SetPauseGame(true);

            var adUnits = new List<string>(1);

            if (Banner != null)
            {
                adUnits.Add(IronSourceAdUnits.BANNER);
            }

            IronSource.Agent.init(config.AppKey, adUnits.ToArray());

#if UNITY_EDITOR
            OnSdkInitializationCompleted();
#endif
        }

        private void OnSdkInitializationCompleted()
        {
            Banner.Init();
        }

        private void OnImpressionDataReady(IronSourceImpressionData impressionData)
        {
            if (impressionData.revenue == null || impressionData.revenue == 0)
                return;

            Debug.Log($"[{nameof(IronSourceAdService)}] OnImpressionDataReady: {impressionData.adUnit}");
        }
    }
}
