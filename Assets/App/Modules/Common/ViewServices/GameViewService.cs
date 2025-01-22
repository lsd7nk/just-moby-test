using App.Common.Views;
using App.Popups;
using App.Core;
using App.Ads;
using System;
using Cysharp.Threading.Tasks;

namespace App.Common
{
    public sealed class GameViewService : ViewService<GameView>, IDisposable
    {
        private readonly IPopupFactoryService _popupFactory;
        private readonly AppConfig _appConfig;

        private readonly IFigureModelsFactory _figureModelsFactory;
        private readonly IFiguresBuilder _figuresBuilder;

        private readonly LevelModel _levelModel;

        public GameViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig appConfig, AppStateService appStates)
        {
            _popupFactory = popupFactory;
            _appConfig = appConfig;

            _levelModel = new LevelModel(_appConfig.FiguresCount);
            _figureModelsFactory = new FigureModelsHSVFactory();
            _figuresBuilder = new FiguresBuilder();
        }

        public override void Initialize()
        {
            _figuresBuilder.SetView(_view.FiguresBuilderView);
            _figuresBuilder.Initialize();

            _figuresBuilder.OnFigureTakeFromScrollEvent += OnFigureTakeFromScroll;

            CreateFigures();
        }

        public void Dispose()
        {
            _figuresBuilder.OnFigureTakeFromScrollEvent -= OnFigureTakeFromScroll;
            _figuresBuilder.Dispose();
        }

        protected override void OnViewSet()
        {
            _view.Initialize(_appConfig.FiguresCount);
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void CreateFigures()
        {
            int figuresCount = _levelModel.FiguresCount;
            var figures = _figureModelsFactory.GetFigureModels(figuresCount);

            for (int i = 0; i < figuresCount; ++i)
            {
                var figure = figures[i];
                var figureView = _view.CreateFigureView(figure.Color, i);

                figure.SetView(figureView);

                _figuresBuilder.AddFigure(figure);
            }
        }

        private void CopyFigure(FigureModel figure)
        {
            var copiedFigure = _figureModelsFactory.GetFigureModel(figure.Color, figure.Index);
            var figureView = _view.CopyFigureView(copiedFigure.Color, copiedFigure.Index);

            copiedFigure.SetView(figureView);

            _figuresBuilder.AddFigure(copiedFigure);
        }

        private void OnFigureTakeFromScroll(FigureModel figure)
        {
            CopyFigure(figure);
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
