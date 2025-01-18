using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace App.Common.Views
{
    public abstract class StateView : MonoBehaviour
    {
        public UniTask HideAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}