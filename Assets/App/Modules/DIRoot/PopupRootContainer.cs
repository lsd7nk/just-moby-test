using MiniContainer;
using UnityEngine;
using App.Popups;

namespace App
{
    public sealed class PopupRootContainer : RootContainer
    {
        [SerializeField] private PopupView[] _popups;

        protected override void Register(IBaseDIService builder)
        {
            builder.RegisterInstance<IPopupFactoryService>(new PopupFactoryService(DIContainer));

            for (int i = 0; i < _popups.Length; ++i)
            {
                var popup = _popups[i];

                builder.RegisterComponentInNewPrefab(popup);
                builder.Register(popup.ServiceType, ServiceLifeTime.Transient);
            }
        }
    }
}