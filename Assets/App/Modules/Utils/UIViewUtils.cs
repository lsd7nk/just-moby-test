using Cysharp.Threading.Tasks;
using System.Threading;
using Doozy.Engine.UI;

namespace App.Utils
{
    public static class UIViewUtils
    {
        public static async UniTask HideAsync(UIView view, CancellationToken cancellationToken)
        {
            if (view == null)
            {
                return;
            }

            bool finished = false;

            view.HideBehavior.OnFinished.Action += (go) =>
            {
                finished = true;
            };

            view.Hide();

            await UniTask.WaitUntil(() => finished, PlayerLoopTiming.Update, cancellationToken);
        }

        public static async UniTask ShowAsync(UIView view, CancellationToken cancellationToken)
        {
            if (view == null)
            {
                return;
            }

            bool finished = false;

            view.ShowBehavior.OnFinished.Action += (go) =>
            {
                finished = true;
            };

            view.Show();

            await UniTask.WaitUntil(() => finished, PlayerLoopTiming.Update, cancellationToken);
        }
    }
}
