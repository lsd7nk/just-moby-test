using Cysharp.Threading.Tasks;
using App.Common.Views;
using System.Threading;

namespace App.Common
{
    public sealed class LobbyViewService : IViewService<LobbyView>
    {
        public void SetView(LobbyView view)
        {

        }

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}
