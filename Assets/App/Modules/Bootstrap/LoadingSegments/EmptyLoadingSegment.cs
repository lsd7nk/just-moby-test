using Cysharp.Threading.Tasks;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class EmptyLoadingSegment : LoadingSegment
    {
        public override async UniTask Init()
        {
            await UniTask.Delay(6000);
        }
    }
}
