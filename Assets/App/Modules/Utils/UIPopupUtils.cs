using Cysharp.Threading.Tasks;
using Doozy.Engine.UI;

namespace App.Utils
{
    public static class UIPopupUtils
    {
        public static async UniTask HideAsync(UIPopup popup)
        {
            if (popup == null)
            {
                return;
            }

            bool finished = false;

            popup.HideBehavior.OnFinished.Action += (go) =>
            {
                finished = true;
            };

            popup.Hide();

            await UniTask.WaitUntil(() => finished, PlayerLoopTiming.Update);
        }

        public static async UniTask ShowAsync(UIPopup popup)
        {
            if (popup == null)
            {
                return;
            }

            bool finished = false;

            popup.ShowBehavior.OnFinished.Action += (go) =>
            {
                finished = true;
            };

            popup.Show();

            await UniTask.WaitUntil(() => finished, PlayerLoopTiming.Update);
        }
    }
}
