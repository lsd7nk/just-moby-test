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
        private readonly IFiguresBuilder _figuresBuilder;

        private readonly LevelModel _levelModel;

        public GameViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig config, AppStateService appStates)
        {
            _popupFactory = popupFactory;

            _figureModelsFactory = new FigureModelsHSVFactory();
            _levelModel = new LevelModel(config.FiguresCount);
            _figuresBuilder = new FiguresBuilder();
        }

        public override void Initialize()
        {
            _figuresBuilder.SetView(_view.FiguresBuilderView);
            _figuresBuilder.Initialize();

            CreateFigures();
        }

        protected override void OnViewSet()
        {
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void CreateFigures()
        {
            int figuresCount = _levelModel.FiguresCount;
            var figureModels = _figureModelsFactory.GetFigureModels(figuresCount);

            for (int i = 0; i < figuresCount; ++i)
            {
                var figureModel = figureModels[i];
                var figureView = _view.CreateFigureView(figureModel.Color);

                figureModel.SetView(figureView);

                _figuresBuilder.AddFigure(figureModel);
            }
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
