using App.Vibrations;
using MiniContainer;
using App.AppStates;
using StateMachine;
using App.Common;
using App.Utils;
using App.Data;
using App.Ads;

namespace App
{
    public sealed class AppRootContainer : RootContainer, IContainerApplicationFocusListener
    {
        public void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                return;
            }

            DIContainer.Resolve<UserDataService>().Save();
        }

        protected override void Register(IBaseDIService builder)
        {
            NativeHeapUtils.ReserveMegabytes(10);

            // State machine & app states
            builder.RegisterStateMachine<AppStateType>(DIContainer);
            builder.RegisterSingleton<AppStateService>();
            builder.RegisterScoped<LoadingAppState>();
            builder.RegisterScoped<LobbyAppState>();
            builder.RegisterScoped<GameAppState>();

            // View services
            builder.RegisterScoped<LoadingViewService>();
            builder.RegisterScoped<LobbyViewService>();
            builder.RegisterScoped<GameViewService>();

            builder.RegisterSingleton<IFirebaseDependenciesService, FirebaseDependenciesService>();
            builder.RegisterSingleton<AdInitializationService>();
            builder.RegisterSingleton<IAdService, AdService>();
            builder.RegisterSingleton<VibrationsService>();
            builder.RegisterSingleton<UserDataService>();
            builder.RegisterSingleton<AppConfig>();
        }

        protected override void Resolve()
        {
            DIContainer.Resolve<UserDataService>();
            DIContainer.Resolve<VibrationsService>();
        }
    }
}