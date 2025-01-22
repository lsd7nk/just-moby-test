using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common.Views;

namespace App.Common
{
    public abstract class ViewService<TView> : IViewService<TView> where TView : StateView
    {
        protected TView _view;

        public void SetView(TView view)
        {
            _view = view;

            OnViewSet();
        }

        public virtual async UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            await _view.HideAsync(cancellationToken);
        }

        public virtual void Initialize() { }

        protected virtual void OnViewSet() { }
    }
}