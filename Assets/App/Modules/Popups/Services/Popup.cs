using Cysharp.Threading.Tasks;
using System;

namespace App.Popups
{
    public abstract class Popup<TView> : IPopup where TView : IPopupView
    {
        public Type ViewType => typeof(TView);

        protected IPopupView _view;

        public virtual void Initialize(IPopupView view, params object[] args)
        {
            _view = view;
        }

        public void Hide()
        {
            _view.HideAsync().Forget();
        }
    }
}
