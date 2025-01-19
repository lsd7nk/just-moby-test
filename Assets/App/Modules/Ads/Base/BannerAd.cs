using App.Events;

namespace App.Ads
{
    public abstract class BannerAd :
#if UNITY_EDITOR
        DummyBannerAd
#else
		AbstractBaseBannerAd
#endif

    {

    }


    public abstract class AbstractBaseBannerAd : IBannerAd
    {
        private bool _allowed = true;
        public bool Allowed
        {
            get => _allowed;
            set
            {
                if (_allowed != value)
                {
                    _allowed = value;
                    if (_allowed)
                    {
                        InvokeLoad();
                    }
                }
            }
        }

        protected bool _canChangeState = true;
        public bool CanChangeState
        {
            get => _canChangeState;
            set
            {
                _canChangeState = value;
                Refresh();
            }
        }

        protected bool _visibleByContext = false;
        public bool VisibleByContext
        {
            get => _visibleByContext;
            set
            {
                _visibleByContext = value;
                Refresh();
            }
        }

        protected bool _isShowing;
        public bool IsShowing => _isShowing;

        public void Init()
        {
            InvokeInternalInit();

            if (_isShowing)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Refresh()
        {
            if (VisibleByContext && !_isShowing && CanChangeState && Allowed)
            {
                Show();
            }
            else if (_isShowing && !Allowed)
            {
                Hide();
            }
        }

        public void Show()
        {
            if (!Allowed)
            {
                return;
            }

            DoShow();
        }

        private void DoShow()
        {
            EventSystem.Send<BannerShownEvent>();

            _isShowing = true;

            InvokeInternalShow();
        }

        public void Hide()
        {
            EventSystem.Send<BannerHiddenEvent>();

            _isShowing = false;

            InvokeInternalHide();
        }

        protected virtual void Load()
        {
        }

        protected abstract void InternalInit();
        protected abstract void InternalShow();
        protected abstract void InternalHide();

        // Dummy overrides these methods
        protected virtual void InvokeInternalInit() => InternalInit();
        protected virtual void InvokeInternalShow() => InternalShow();
        protected virtual void InvokeInternalHide() => InternalHide();
        protected virtual void InvokeLoad() => Load();
    }
}