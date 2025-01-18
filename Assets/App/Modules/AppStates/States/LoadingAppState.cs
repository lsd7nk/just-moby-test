using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common;

namespace App.AppStates
{
    public sealed class LoadingAppState : AppState<AppStatePayload>
    {
        private readonly LoadingViewService _viewService;

        public LoadingAppState(AppStateAssets appStateAssets, LoadingViewService loadingService) : base(appStateAssets)
        {
            _viewService = loadingService;
        }

        public override async UniTask OnEnter(AppStateType trigger, AppStatePayload payload, CancellationToken cancellationToken)
        {
            await base.OnEnter(trigger, payload, cancellationToken);

            PopulateViewService(_viewService);
        }

        public override async UniTask OnExit(AppStateType currentTrigger, AppStateType nextTrigger, CancellationToken cancellationToken)
        {
            await _viewService.HideViewAsync(cancellationToken);
        }
    }
}
