using App.Common.Views;
using App.Popups;
using App.Core;
using App.Ads;

namespace App.Common
{
    public sealed class GameViewService : ViewService<GameView>
    {
        private readonly IFigureModelsFactory _figureModelsFactory;
        private readonly IPopupFactoryService _popupFactory;

        private readonly LevelModel _levelModel;

        public GameViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig config, AppStateService appStates)
        {
            _popupFactory = popupFactory;

            _figureModelsFactory = new FigureModelsHSVFactory();
            _levelModel = new LevelModel(config.FiguresCount);
        }

        public override void Initialize()
        {
            CreateFigures();
        }

        protected override void OnViewSet()
        {
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void OnFigureDragStarted(DraggableObject draggable)
        {
            UnityEngine.Debug.Log("OnFigureDragStarted");
        }

        private void OnFigureDragEnded(DraggableObject draggable)
        {
            UnityEngine.Debug.Log("OnFigureDragEnded");
        }

        private void CreateFigures()
        {
            int figuresCount = _levelModel.FiguresCount;
            var figureModels = _figureModelsFactory.GetFigureModels(figuresCount);

            for (int i = 0; i < figuresCount; ++i)
            {
                var figureView = _view.CreateFigureView();
                var draggable = figureView.GetDraggable();

                figureView.SetColor(figureModels[i].Color);

                AddFigureEventsListener(draggable);
            }
        }

        private void AddFigureEventsListener(DraggableObject draggable)
        {
            draggable.OnDragStartedEvent += OnFigureDragStarted;
            draggable.OnDragEndedEvent += OnFigureDragEnded;
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
