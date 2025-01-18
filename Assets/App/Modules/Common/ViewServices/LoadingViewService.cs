using Cysharp.Threading.Tasks;
using App.Common.Views;
using System.Threading;

namespace App.Common
{
    public sealed class LoadingViewService : IViewService<LoadingView>
    {
        public void SetLoadingProgress(float value)
        {

        }

        public void SetView(LoadingView view)
        {

        }

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}
