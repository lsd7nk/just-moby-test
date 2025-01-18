using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common;

namespace App.AppStates
{
    public sealed class LobbyAppState : AppState<AppStatePayload>
    {
        private readonly LobbyViewService _viewService;

        public LobbyAppState(AppStateAssets appStateAssets, LobbyViewService viewService) : base(appStateAssets)
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