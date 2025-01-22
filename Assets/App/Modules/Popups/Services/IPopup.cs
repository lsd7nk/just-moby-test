using Cysharp.Threading.Tasks;
using System;

namespace App.Popups
{
    public interface IPopup
    {
        Type ViewType { get; }

        void Initialize(IPopupView view, params object[] args);
        UniTask HideAsync();
        void Hide();
    }
}