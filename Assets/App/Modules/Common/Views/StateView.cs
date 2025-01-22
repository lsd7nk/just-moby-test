using Cysharp.Threading.Tasks;
using System.Threading;
using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;

namespace App.Common.Views
{
    public abstract class StateView : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private UIView[] _uiViews;

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            int viewsCount = _uiViews.Length;
            var tasks = new UniTask[viewsCount];

            for (int i = 0; i < viewsCount; ++i)
            {
                tasks[i] = UIViewUtils.HideAsync(_uiViews[i], cancellationToken);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}