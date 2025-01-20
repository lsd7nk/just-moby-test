using Cysharp.Threading.Tasks;

namespace App.Bootstrap.LoadingSegments
{
    public sealed class FirebaseLoadingSegment : LoadingSegment
    {
        private readonly IFirebaseDependenciesService _firebase;

        public FirebaseLoadingSegment(IFirebaseDependenciesService firebase)
        {
            _firebase = firebase;
        }

        public override async UniTask Init()
        {
            await _firebase.FixDependenciesAsync();
        }
    }
}
