using Cysharp.Threading.Tasks;
using App.Common.Views;
using System.Threading;

namespace App.Common
{
    public sealed class GameViewService : IViewService<GameView>
    {
        public void SetView(GameView view)
        {

        }

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}
