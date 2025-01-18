using Cysharp.Threading.Tasks;
using App.Common.Views;
using System.Threading;

namespace App.Common
{
    public sealed class LoadingViewService : IViewService<LoadingView>
    {
        public void SetView(LoadingView view)
        {
            throw new System.NotImplementedException();
        }

        public UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
