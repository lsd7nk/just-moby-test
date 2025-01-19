using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common.Views;
using App.Ads;

namespace App.AppStates
{
    public abstract class AdsAppState<TPayload> : AppState<TPayload> where TPayload : AppStatePayload
    {
        private readonly IAdService _adService;

        public AdsAppState(AppStateAssets appStateAssets, IAdService adService) : base(appStateAssets)
        {
            _adService = adService;
        }

        public override async UniTask OnEnter(AppStateType trigger, TPayload payload, CancellationToken cancellationToken)
        {
            await base.OnEnter(trigger, payload, cancellationToken);

            for (int i = 0; i < _instances.Length; ++i)
            {
                var instance = _instances[i];

                if (!instance.TryGetComponent<AdsView>(out var adsView))
                {
                    continue;
                }

                adsView.BannerAdjuster.Initialize(_adService);
            }
        }
    }
}
