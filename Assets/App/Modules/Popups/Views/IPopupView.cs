using Cysharp.Threading.Tasks;
using System;

namespace App.Popups
{
    public interface IPopupView
    {
        Type ServiceType { get; }

        UniTask HideAsync();
    }
}