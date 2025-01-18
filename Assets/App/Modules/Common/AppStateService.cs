using Cysharp.Threading.Tasks;
using System.Threading;
using App.AppStates;
using StateMachine;

namespace App.Common
{
    public sealed class AppStateService
    {
        private readonly IStateMachine<AppStateType> _stateMachine;

        public AppStateService(IStateMachine<AppStateType> stateMachine)
        {
            _stateMachine = stateMachine;

            Initialize();
            GoToState(AppStateType.Loading).Forget();
        }

        public async UniTask GoToState(AppStateType state)
        {
            await GoToState<AppStatePayload>(state, null);
        }

        public async UniTask GoToState<TPayload>(AppStateType state, TPayload payload)
            where TPayload : IStatePayload
        {
            await _stateMachine.Fire(state, payload, CancellationToken.None);
        }

        private void Initialize()
        {
            _stateMachine.Register<LoadingAppState>(AppStateType.Loading)
                .AllowTransition(AppStateType.Lobby);

            _stateMachine.Register<LobbyAppState>(AppStateType.Lobby)
                .AllowTransition(AppStateType.Game);

            _stateMachine.Register<GameAppState>(AppStateType.Game)
                .AllowTransition(AppStateType.Lobby);
        }
    }
}