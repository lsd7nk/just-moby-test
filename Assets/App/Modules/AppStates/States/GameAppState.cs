using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common;

namespace App.AppStates
{
    public sealed class GameAppState : AppState<AppStatePayload>
    {
        private readonly GameViewService _viewService;

        public GameAppState(AppStateAssets appStateAssets, GameViewService viewService) : base(appStateAssets)
        {
            _viewService = viewService;
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
