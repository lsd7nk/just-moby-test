using MiniContainer;
using App.AppStates;
using StateMachine;
using App.Utils;

namespace App
{
    public sealed class AppRootContainer : RootContainer
    {
        protected override void Register(IBaseDIService builder)
        {
            NativeHeapUtils.ReserveMegabytes(10);

            builder.RegisterStateMachine<AppStateType>(DIContainer);
        }
    }
}