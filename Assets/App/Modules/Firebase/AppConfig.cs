using App.Settings;
using System;

namespace App
{
    public sealed class AppConfig : FirebaseConfig
    {
        public bool BannerAdEnabled { get; private set; }

        public AppConfig(IFirebaseDependenciesService firebase) : base(firebase, TimeSpan.FromSeconds(4)) { }

        protected override void OnSuccessfulCloudLoad()
        {
            base.OnSuccessfulCloudLoad();

            BannerAdEnabled = GetBoolValue(nameof(BannerAdEnabled));
        }

        protected override void OnFailedCloudLoad()
        {
            base.OnFailedCloudLoad();

            var settings = SettingsProvider.Get<ConfigSettings>();

            BannerAdEnabled = settings.BannerAdEnabled;
        }
    }
}