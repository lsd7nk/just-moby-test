using System;

namespace App.Popups
{
    public interface IPopup
    {
        Type ViewType { get; }

        void Initialize(IPopupView view, params object[] args);
        void Hide();
    }
}