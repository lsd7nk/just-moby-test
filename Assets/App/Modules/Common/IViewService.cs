using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace App.Common
{
    public interface IViewService<TView> : IViewService where TView : MonoBehaviour
    {
        void SetView(TView view);
    }

    public interface IViewService
    {
        void Initialize();
        UniTask HideViewAsync(CancellationToken cancellationToken);
    }
}