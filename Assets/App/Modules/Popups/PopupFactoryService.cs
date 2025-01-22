using MiniContainer;

namespace App.Popups
{
    public interface IPopupFactoryService
    {
        TService ShowPopup<TService>(params object[] args) where TService : IPopup;
    }


    public sealed class PopupFactoryService : IPopupFactoryService
    {
        private readonly IContainer _container;

        public PopupFactoryService(IContainer container)
        {
            _container = container;
        }

        public TService ShowPopup<TService>(params object[] args) where TService : IPopup
        {
            var service = _container.Resolve<TService>();

            if (service != null)
            {
                var viewType = service.ViewType;
                var view = (IPopupView)_container.Resolve(viewType);

                service.Initialize(view, args);
            }

            return service;
        }
    }
}
