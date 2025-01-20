using Cysharp.Threading.Tasks;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class AppConfigLoadingSegment : LoadingSegment
    {
        private readonly AppConfig _config;

        public AppConfigLoadingSegment(AppConfig config)
        {
            _config = config;
        }

        public override async UniTask Init()
        {
            _config.Initialize();

            await UniTask.WaitUntil(() => _config.Initialized);
        }
    }
}